using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public GameObject imagePrefab; // 배치할 이미지 프리팹
    public int count = 5; // 생성할 이미지의 개수
    public float xSpacing = 10f; // 이미지 간 x축 간격
    public float ySpacing = 10f; // 이미지 간 y축 간격

    void Start()
    {
        // 캔버스의 RectTransform 가져오기
        RectTransform canvasRect = GetComponent<RectTransform>();

        // 이미지 생성 및 배치
        for (int i = 0; i < count; i++)
        {
            // 이미지 프리팹 생성
            GameObject newImage = Instantiate(imagePrefab, Vector3.zero, Quaternion.identity);

            // 생성된 이미지의 RectTransform 가져오기
            RectTransform rt = newImage.GetComponent<RectTransform>();

            // 이미지 위치 설정 (다른 이미지들과 겹치지 않는 위치로 설정)
            Vector2 newPosition = FindNonOverlappingPosition(canvasRect, rt.sizeDelta);
            rt.anchoredPosition = newPosition;

            // 생성된 이미지를 캔버스의 자식으로 설정 (이렇게 하면 캔버스 위에 올라가게 됨)
            newImage.transform.SetParent(transform, false);
        }
    }

    // 다른 이미지들과 겹치지 않는 위치를 찾는 함수
    Vector2 FindNonOverlappingPosition(RectTransform canvasRect, Vector2 size)
    {
        Vector2 newPosition;
        bool foundNonOverlappingPosition = false;
        do
        {
            // 0에서 캔버스의 너비와 높이 사이의 랜덤한 위치 생성
            float newX = Random.Range(0f, 800f);
            float newY = Random.Range(0f, 400f);
            newPosition = new Vector2(newX, newY);

            // 새로운 위치에서 이미지가 겹치는지 확인
            foundNonOverlappingPosition = true;
            foreach (RectTransform child in canvasRect)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(child, newPosition, null))
                {
                    foundNonOverlappingPosition = false;
                    break;
                }
            }
        } while (!foundNonOverlappingPosition);

        return newPosition;
    }
}
