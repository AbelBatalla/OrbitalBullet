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

    public GameObject textLoot;
    private uiSingleton singleton_ui;
    private GameObject text_kill_first;

    void Start()
    {
        open = false;
        killsCounter = GameObject.FindWithTag("KillsCanvas").GetComponent<KillsCounter>();
        audioPlayer = GetComponent<AudioSource>();
        GameObject singletonObject = GameObject.Find("UISingleton");
        singleton_ui = singletonObject.GetComponent<uiSingleton>();
        text_kill_first = singleton_ui.getChestEnemyText();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !open)
        {
            text.SetActive(true);
            if (!open && Input.GetKey(KeyCode.T))
            {
                if(killsCounter?.getSouls()>0) {
                    open = true;
                    if (animator != null) animator.activa();
                    audioPlayer.PlayOneShot(openAudio);
                    killsCounter?.consumeSoul();
                    Invoke("ShowItem", 0.7f);
                    text.SetActive(false);
                } else {
                    text_kill_first.SetActive(true);
                    Invoke("DisableKillFirst", 1.0f);
                }
            }
        }
    }

    void DisableKillFirst(){
         text_kill_first.SetActive(false);
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