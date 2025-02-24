using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BlockManager : MonoBehaviour
{
    private static BlockManager _instance;
    public static BlockManager Instance { get { return _instance; } }

    [SerializeField] private Block blackBlockPrefab;
    [SerializeField] private Block grayBlockPrefab;
    [SerializeField] private float spaceScale;
    public Transform blockSpawnPos;
    [SerializeField] private float autoDelay;

    private List<Block> childBlock = new List<Block>();
    public List<Block> confirmedBlock = new List<Block>();
    private List<Block> tempBlock = new List<Block>();

    public string stageName = "";

    private Block _lastWordPrefab;
    private float _blockLength;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        stageName = SceneManager.GetActiveScene().name;
    }

    public void MakeFirstBlock(string word)
    {
        Vector3 movePos = Vector3.zero;
        for (int i = 0; i < word.Length; i++)
        {
            Block block;
            block = Instantiate(blackBlockPrefab, blockSpawnPos);

            block.transform.position += movePos + Vector3.up * spaceScale;
            block.SetWord(word[i], '\0', false);

            confirmedBlock.Add(block);

            movePos += Vector3.right;
        }
    }

    public void MakeBlock(string preWord, string word)
    {
        _blockLength = Block.blockLength;
        if (word == null)
        {
            Debug.LogWarning("입력된 단어 없음");
            return;
        }

        //블록 초기화
        InitializeBlockPrefab(word);

        //마지막 글자가 두음법칙을 만족하는가?
        bool hasRuleOfHeading = HEEJAEGameManager.Instance._ruleOfHeading.ContainsKey(preWord[preWord.Length - 1]);
        //마지막 글자
        char lastChar = preWord[preWord.Length - 1];
        //두음법칙 적용된 글자
        char ruledChar = hasRuleOfHeading ? HEEJAEGameManager.Instance._ruleOfHeading[lastChar] : '\0';

        Vector3 movePos = Vector3.zero;

        //오른쪽 최대 위치
        float screenRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        //블록이 처음 생성되는 위치
        float startX = blockSpawnPos.position.x;

        float currentX = startX;
        float currentY = blockSpawnPos.position.y;

        for (int i = 0; i < word.Length; i++)
        {
            Block block;
            block = Instantiate(blackBlockPrefab, blockSpawnPos);
            bool isStart = (i == 0);

            //첫번째 글자이면서 두음법칙을 만족하면
            if (isStart && hasRuleOfHeading && (word[0] == lastChar || word[0] == ruledChar))
            {
                block.SetWord(lastChar, ruledChar, true);
            }
            else
            {
                block.SetWord(word[i], ruledChar, false);
            }

            if (currentX + _blockLength >= screenRightEdge)
            {
                currentX = blockSpawnPos.position.x;
                currentY -= _blockLength;
            }

            block.transform.position = new Vector3(currentX, currentY, blockSpawnPos.position.z);

            childBlock.Add(block);
            currentX += _blockLength;
            //movePos += Vector3.right * _blockLength;

            //Invoke("Temp", 3f);
        }
    }

    public void AutoBlock(string preWord, string word)
    {
        temp = false;
        if (word == null)
        {
            Debug.LogWarning("입력된 단어 없음");
            return;
        }

        //블록 초기화
        InitializeBlockPrefab(word);

        StartCoroutine(HandleAutoBlock(preWord, word));
    }

    public bool temp = false;
    private IEnumerator HandleAutoBlock(string preWord, string word)
    {
        _blockLength = Block.blockLength;
        //마지막 글자가 두음법칙을 만족하는가?
        bool hasRuleOfHeading = HEEJAEGameManager.Instance._ruleOfHeading.ContainsKey(preWord[preWord.Length - 1]);
        //마지막 글자
        char lastChar = preWord[preWord.Length - 1];
        //두음법칙 적용된 글자
        char ruledChar = hasRuleOfHeading ? HEEJAEGameManager.Instance._ruleOfHeading[lastChar] : '\0';

        Vector3 movePos = Vector3.zero;

        //오른쪽 최대 위치
        float screenRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        //블록이 처음 생성되는 위치
        float startX = blockSpawnPos.position.x;

        float currentX = startX;
        float currentY = blockSpawnPos.position.y;

        for (int i = 0; i < word.Length; i++)
        {
            Block block;
            block = Instantiate(blackBlockPrefab, blockSpawnPos);
            bool isStart = (i == 0);

            //첫번째 글자이면서 두음법칙을 만족하면
            if (isStart && hasRuleOfHeading && (word[0] == lastChar || word[0] == ruledChar))
            {
                block.SetWord(lastChar, ruledChar, true);
            }
            else
            {
                block.SetWord(word[i], ruledChar, false);
            }

            if (currentX + _blockLength >= screenRightEdge)
            {
                currentX = blockSpawnPos.position.x;
                currentY -= _blockLength;
            }

            block.transform.position = new Vector3(currentX, currentY, blockSpawnPos.position.z);

            childBlock.Add(block);
            currentX += _blockLength;
            //block.transform.position += movePos;

            //childBlock.Add(block);

            //movePos += Vector3.right * _blockLength;

            yield return new WaitForSeconds(autoDelay);
        }
        ConfirmBlock();
        temp = true;
    }

    public void MakeSuggestionBlock(string preWord, string typingWord, string currentSuggestion)
    {
        _blockLength = Block.blockLength;
        if (currentSuggestion == null)
        {
            Debug.LogError("추천 단어 안넘어옴");
            return;
        }

        print($"추천단어 : {currentSuggestion}");
        InitializeBlockPrefab(currentSuggestion);

        Vector3 movePos = Vector3.zero;

        //이전 단어가 두음법칙을 만족했는가
        bool hasRuleOfHeading = HEEJAEGameManager.Instance._ruleOfHeading.ContainsKey(preWord[preWord.Length - 1]);
        //두음 법칙이 성립되는 이전 단어
        char lastChar = preWord[preWord.Length - 1];
        //이전 단어에 두음 법칙 적용 후 단어
        char ruledChar = hasRuleOfHeading ? HEEJAEGameManager.Instance._ruleOfHeading[lastChar] : '\0';

        //오른쪽 최대 위치
        float screenRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        //블록이 처음 생성되는 위치
        float startX = blockSpawnPos.position.x;

        float currentX = startX;
        float currentY = blockSpawnPos.position.y;

        for (int i = 0; i < currentSuggestion.Length; i++)
        {
            Block block;
            if (i < typingWord.Length && (currentSuggestion[i] == typingWord[i] || currentSuggestion[i] == lastChar))
            {
                block = Instantiate(blackBlockPrefab, blockSpawnPos);
            }
            else
            {
                block = Instantiate(grayBlockPrefab, blockSpawnPos);
            }

            bool isStart = (i == 0);

            //첫번째 글자이면서 두음법칙을 만족하고, 내가 치는 단어의 처음이 두음법칙일 경우
            if(isStart && hasRuleOfHeading && (typingWord[0] == lastChar || typingWord[0] == ruledChar))
            {
                block.SetWord(lastChar, ruledChar, true);
            }
            else
            {
                block.SetWord(currentSuggestion[i], ruledChar, false);
            }

            if (currentX + _blockLength >= screenRightEdge)
            {
                currentX = blockSpawnPos.position.x;
                currentY -= _blockLength;
            }

            block.transform.position = new Vector3(currentX, currentY, blockSpawnPos.position.z);

            childBlock.Add(block);
            currentX += _blockLength;

            //block.transform.position += movePos;
            //childBlock.Add(block);

            //movePos += Vector3.right * _blockLength;
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
        float childBlockY = childBlock[childBlock.Count() - 1].transform.position.y;
        Vector3 upScale = Vector3.up * childBlockY;
        foreach (var block in childBlock)
        {
            block.transform.position -= upScale;
            block.SpawnParticle();
        }

        confirmedBlock.AddRange(childBlock);
        childBlock.Clear();
        foreach(var block in confirmedBlock)
        {
            block.transform.position += Vector3.up * spaceScale;
            block.word.color = Color.black;
        }
    }

    public void SetPrefabTextRed()
    {
        tempBlock.AddRange(childBlock);
        childBlock.Clear();
        StartCoroutine(HandleTextRedTime());
    }

    private IEnumerator HandleTextRedTime()
    {
        foreach (var block in tempBlock)
        {
            block.word.color = Color.red;
            print($"빨개짐");
        }
        yield return new WaitForSeconds(0.3f);
        foreach (var block in tempBlock)
        {
            Destroy(block.gameObject);
        }
        tempBlock.Clear();
    }
}

