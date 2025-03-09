using UnityEngine;

public class BombManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            UIManager.Instance.UseBomb();
        }
    }
}
