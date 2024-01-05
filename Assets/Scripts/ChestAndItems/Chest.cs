 using UnityEngine;

public class Chest : MonoBehaviour
{
    public WeightedRandomList<Transform> lootTable;
    public Transform itemHolder;
    bool open;
    public animatorScript animator;
    public KillsCounter killsCounter;
    public AudioClip openAudio;
    private AudioSource audioPlayer;

    public GameObject text;

    void Start()
    {
        open = false;
        killsCounter = GameObject.FindWithTag("KillsCanvas").GetComponent<KillsCounter>();
        audioPlayer = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !open)
        {
            text.SetActive(true);
            if (!open && Input.GetKey(KeyCode.T) && killsCounter?.getSouls()>0)
            {
                open = true;
                if (animator != null) animator.activa();
                audioPlayer.PlayOneShot(openAudio);
                killsCounter?.consumeSoul();
                Invoke("ShowItem", 0.7f);
                text.SetActive(false);
            }
        }
    }

     private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }

    void ShowItem()
    {
        Transform item = lootTable.GetRandom();
        Instantiate(item, itemHolder);
        itemHolder.gameObject.SetActive(true);
    }
}