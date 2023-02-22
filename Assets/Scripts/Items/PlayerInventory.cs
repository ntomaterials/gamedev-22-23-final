using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour // Сделаю по-колхозному. Надеюсь, вряд ли будет потом сто-то кроме мяса
{
    protected List<Product> products=new List<Product>();
    [field: SerializeField] public int maxCapacity { get; protected set; }
    public Product baseProduct;
    [Header("Canvas")]
    [SerializeField] protected Image icon;
    [SerializeField] protected Text countText;
    //private Transform _dropPoint;
    protected virtual void Start()
    {
        UpdateCanvas();
        //_dropPoint = Player.Instance.dropPoint;
        InputHandler.Instance.onDropBtnUp += DropItem;
    }
    public bool CanAdd()
    {
        if (products.Count < maxCapacity) return true;
        else return false;
    }
    public void AddProduct(Product product)
    {
        products.Add(product);
        UpdateCanvas();
    }
    public void LoadInventory(int count)
    {
        products = new List<Product>();
        for (int i = 0; i < count; i++) products.Add(baseProduct);
        UpdateCanvas();
    }
    public void DropItem()
    {
        if (products.Count > 0)
        {
            Product droped = Instantiate(baseProduct, Player.Instance.dropPoint.position, Quaternion.identity);
            droped.transform.parent = null;
            products.RemoveAt(products.Count - 1);
            UpdateCanvas();
        }
    }
    protected void UpdateCanvas()
    {
        if (products.Count == 0)
        {
            icon.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
            countText.gameObject.SetActive(true);
            countText.text = products.Count.ToString();
        }
    }
}
