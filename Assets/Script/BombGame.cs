using System.Collections;
using UnityEngine;

public class BombGame : MonoBehaviour
{
    public GameObject[] characters; // 5명의 캐릭터 오브젝트를 배열로 저장
    private GameObject bomb; // 폭탄 오브젝트
    private int currentPlayerIndex; // 현재 폭탄을 가진 캐릭터의 인덱스

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // 캐릭터들을 초기 위치에 배치
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].transform.position = new Vector3(i * 2.0f, 0.0f, 0.0f);
        }

        // 폭탄 오브젝트 생성 및 초기 위치 설정
        bomb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bomb.transform.position = characters[0].transform.position;

        currentPlayerIndex = 0; // 초기에 폭탄을 가진 캐릭터는 첫 번째 캐릭터
        StartCoroutine(BombTimerCoroutine());
    }

    private IEnumerator BombTimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);

            // 폭탄을 가진 캐릭터가 폭탄을 다른 캐릭터에게 전달
            PassBomb();
        }
    }

    private void PassBomb()
    {
        int nextPlayerIndex;
        do
        {
            // 다음 폭탄을 가진 캐릭터의 인덱스 설정 (현재 폭탄을 가진 플레이어를 제외)
            nextPlayerIndex = Random.Range(0, characters.Length);
        } while (nextPlayerIndex == currentPlayerIndex);

        // 이전 폭탄을 가진 캐릭터의 위치에서 다음 폭탄을 가진 캐릭터에게 전달
        Vector3 targetPosition = characters[nextPlayerIndex].transform.position;
        StartCoroutine(MoveBombCoroutine(targetPosition));

        currentPlayerIndex = nextPlayerIndex; // 다음 폭탄을 가진 캐릭터의 인덱스로 업데이트
    }

    private IEnumerator MoveBombCoroutine(Vector3 targetPosition)
    {
        float elapsedTime = 0.0f;
        float duration = 2.0f; // 폭탄의 이동 시간

        Vector3 startPosition = bomb.transform.position;

        while (elapsedTime < duration)
        {
            bomb.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bomb.transform.position = targetPosition; // 이동 완료 후 위치 보정
    }
}