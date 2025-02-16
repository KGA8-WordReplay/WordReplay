using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockManager : MonoBehaviour
{
    private static BlockManager _instance;
    public static BlockManager Instance { get { return _instance; } }

    [SerializeField] private Block blackBlockPrefab;
    [SerializeField] private Block grayBlockPrefab;

    private List<Block> childBlock = new List<Block>();

    private Block _lastWordPrefab;

    private void Awake()
    {
        _instance = this;
    }

    private string _preSuggestion = "";
    public void MakeSuggestionBlock(string typingWord, string currentSuggestion)
    {
        if (currentSuggestion == null)
        {
            Debug.LogError("추천 단어 안넘어옴");
            return;
        }
        DestroyBlock(typingWord);

        Vector3 movePos = Vector3.right;

        for(int i = 0; i < currentSuggestion.Length; i++)
        {
            Block block;
            if (i < typingWord.Length && currentSuggestion[i] == typingWord[i])
            {
                block = Instantiate(blackBlockPrefab);
            }
            else
            {
                block = Instantiate(grayBlockPrefab);
            }

            block.SetWord(currentSuggestion[i]);
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

    public void DestroyBlock(string typing)
    {
        if (childBlock.Count <= 0)
        {
            return;
        }

        for (int i = childBlock.Count - 1; i >= 0; i--)
        {
            Block block = childBlock[i];

            bool matchFound = false;
            for (int j = typing.Length - 1; j >= 0; j--)
            {
                //현재 입력중인것과 다른 프리팹은 삭제
                if (block.word.text == typing[j].ToString())
                {
                    matchFound = true;
                    break;
                }
            }

            if (!matchFound)
            {
                Destroy(block.gameObject);
                childBlock.Remove(block);
            }
        }
    }

    //끝말이 항상 생성되게 함
    public void MakeLastWord(string preWord)
    {
        char lastWord = preWord[preWord.Length - 1];

        _lastWordPrefab = Instantiate(blackBlockPrefab);
        _lastWordPrefab.SetWord(lastWord);
        Vector3 temp = _lastWordPrefab.transform.position += Vector3.right;
        _lastWordPrefab.transform.position = temp;
    }
}

