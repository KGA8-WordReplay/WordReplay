using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private static BlockManager _instance;
    public static BlockManager Instance { get { return _instance; } }

    [SerializeField] private Block blackBlockPrefab;
    [SerializeField] private Block grayBlockPrefab;

    private List<Block> childBlock = new List<Block>();
    private List<Block> confirmedBlock = new List<Block>();

    private Block _lastWordPrefab;

    private void Awake()
    {
        _instance = this;
    }

    public void MakeBlock(string preWord, string word)
    {
        if (word == null)
        {
            Debug.LogWarning("입력된 단어 없음");
            return;
        }

        InitializeBlockPrefab(word);

        bool hasRuleOfHeading = HEEJAEGameManager.Instance._ruleOfHeading.ContainsKey(preWord[preWord.Length - 1]);
        char lastChar = preWord[preWord.Length - 1];
        char ruledChar = hasRuleOfHeading ? HEEJAEGameManager.Instance._ruleOfHeading[lastChar] : '\0';

        Vector3 movePos = Vector3.right;

        for (int i = 0; i < word.Length; i++)
        {
            Block block;
            block = Instantiate(blackBlockPrefab);
            bool isStart = (i == 0) ? true : false;

            //첫번째 글자이면서 두음법칙을 만족하면
            if (isStart && hasRuleOfHeading && (word[0] == lastChar || word[0] == ruledChar))
            {
                block.SetWord(lastChar, ruledChar, isStart);
            }
            else
            {
                block.SetWord(word[i], ruledChar, false);
            }
            block.transform.position += movePos;
            childBlock.Add(block);

            movePos += Vector3.right;
        }
    }

    public void MakeSuggestionBlock(string preWord, string typingWord, string currentSuggestion)
    {
        if (currentSuggestion == null)
        {
            Debug.LogError("추천 단어 안넘어옴");
            return;
        }

        InitializeBlockPrefab(currentSuggestion);

        Vector3 movePos = Vector3.right;

        //이전 단어가 두음법칙을 만족했는가
        bool hasRuleOfHeading = HEEJAEGameManager.Instance._ruleOfHeading.ContainsKey(preWord[preWord.Length - 1]);
        char lastChar = preWord[preWord.Length - 1];
        char ruledChar = hasRuleOfHeading ? HEEJAEGameManager.Instance._ruleOfHeading[lastChar] : lastChar;

        for (int i = 0; i < currentSuggestion.Length; i++)
        {
            Block block;
            if (i < typingWord.Length && (currentSuggestion[i] == typingWord[i] || currentSuggestion[i] == lastChar))
            {
                block = Instantiate(blackBlockPrefab);
            }
            else
            {
                block = Instantiate(grayBlockPrefab);
            }

            bool isStart = (i == 0) ? true : false;

            //첫번째 글자이면서 두음법칙을 만족하면
            if(isStart && hasRuleOfHeading && (typingWord[0] == lastChar || typingWord[0] == ruledChar))
            {
                block.SetWord(lastChar, ruledChar, isStart);
            }
            else
            {
                block.SetWord(currentSuggestion[i], '\0', false);
            }
            block.transform.position += movePos;
            childBlock.Add(block);

            movePos += Vector3.right;
        }
    }

    //블록 확정시키는 함수
    public void ConfirmedBlock()
    {
        if (childBlock.Count <= 0)
        {
            return;
        }

        foreach (Block block in childBlock)
        {
            block.transform.SetParent(null);
        }
    }

    public void InitializeBlockPrefab(string typing)
    {
        if (childBlock.Count <= 0)
        {
            return;
        }

        for(int i = childBlock.Count - 1; i >= 0; i--)
        {
            Block block = childBlock[i];
            Destroy(block.gameObject);
            childBlock.Remove(block);
        }

        //for (int i = childBlock.Count - 1; i >= 0; i--)
        //{
        //    Block block = childBlock[i];

        //    bool matchFound = false;
        //    for (int j = typing.Length - 1; j >= 0; j--)
        //    {
        //        //현재 입력중인것과 다른 프리팹은 삭제
        //        if (block.word.text == typing[j].ToString())
        //        {
        //            matchFound = true;
        //            break;
        //        }
        //    }

        //    if (!matchFound)
        //    {
        //        Destroy(block.gameObject);
        //        childBlock.Remove(block);
        //    }
        //}
    }


    //끝말이 항상 생성되게 함
    public void MakeLastWord(string preWord)
    {
        string lastWord = preWord[preWord.Length - 1].ToString();

        _lastWordPrefab = Instantiate(blackBlockPrefab);
        //_lastWordPrefab.SetWord(lastWord);
        Vector3 temp = _lastWordPrefab.transform.position += Vector3.right;
        _lastWordPrefab.transform.position = temp;
    }

    private bool CheckRuleOfHeading(char word)
    {
        if (HEEJAEGameManager.Instance._ruleOfHeading.ContainsKey(word))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    //끝말잇기가 되면 올라감
    public void ConfirmBlock()
    {
        confirmedBlock = childBlock.ToListPooled();
    }
}

