using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameEndManager : Singleton<GameEndManager>
{
    private Block _targetBlock;
    private Camera _camera;
    public bool WinAnimEnd = false;

    private void Awake()
    {
        _camera = Camera.main;
    }
    private void Start()
    {
        WinAnimEnd = false;
    }

    public void Win()
    {
        StartCoroutine(WinCameraMove());
    }
    private IEnumerator WinCameraMove()
    {
        _targetBlock = BlockManager.Instance.confirmedBlock[0];
        Vector3 startPos = _camera.transform.position;
        Vector3 endPos = _targetBlock.transform.position + new Vector3(7.69f, 0, -10);

        float elaspedTime = 0f;
        float durationTime = 0.5f;

        yield return new WaitUntil(() => BlockManager.Instance.blockSpawnEnd);

        while (elaspedTime <= durationTime)
        {
            elaspedTime += Time.deltaTime;
            _camera.transform.position = Vector3.Lerp(startPos, endPos, elaspedTime / durationTime);
            yield return null;
        }

        _camera.transform.position = endPos;
        WinAnimEnd = true;
    }

    public void Lose()
    {
        foreach(var block in BlockManager.Instance.confirmedBlock)
        {
            Vector3 random = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            block.rb.useGravity = true;
            block.rb.velocity += random;
        }
    }
}
