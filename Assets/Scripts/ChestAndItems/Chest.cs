 using UnityEngine;

public class Chest : MonoBehaviour
{
    public WeightedRandomList<Transform> lootTable;

    public Transform itemHolder;

    bool open, active;

    void Start()
    {
        open = false;
        active = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (active)
        {
            if (other.gameObject.tag == "Player")
            {
                if (Input.GetKey(KeyCode.T))
                {
                    if (open)
                    {
                        HideItem();
                    }
                    else
                    {
                        ShowItem();
                    }
                    active = false;
                    Invoke("activate", 1f);
                }
            }
        }
    }
    private void activate()
    {
        active = true;
    }

    void HideItem()
    {
        open = false;
        //itemHolder.localScale = Vector3.zero;
        foreach (Transform child in itemHolder)
        {
            Destroy(child.gameObject);
            Debug.Log("Destroying");
        }
        itemHolder.gameObject.SetActive(false);
    }

    void ShowItem()
    {
        open = true;
        Transform item = lootTable.GetRandom();
        Instantiate(item, itemHolder);
        itemHolder.gameObject.SetActive(true);
    }
}