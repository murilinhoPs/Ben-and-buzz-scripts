using UnityEngine;

public class NpcCutscene : MonoBehaviour
{

    public GameObject cutscene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human")) cutscene.SetActive(true);
    }
}
