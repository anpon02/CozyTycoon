using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialCoordinator : MonoBehaviour
{
    Button button;
    [SerializeField] WorkspaceController ws;
    [SerializeField] TextMeshProUGUI recipeName;
    [SerializeField] Image recipeImg;
    Image img;
    Color normalCol;

    private void Start()
    {
        recipeName.text = "";
        img = GetComponent<Image>();
        normalCol = img.color;
        button = GetComponent<Button>();
    }

    public void init(Item item)
    {
        if (item == null) {
            recipeName.gameObject.SetActive(false);
            recipeImg.gameObject.SetActive(false);
            return;
        }
        recipeName.gameObject.SetActive(true);
        recipeImg.gameObject.SetActive(true);
        recipeName.text = item.GetName();
        recipeImg.sprite = item.GetSprite();
    }

    public void Hover()
    {
        if (!string.IsNullOrEmpty(recipeName.text)) ws.chosenRecipe = recipeName.text;
    }

    public void EndHover()
    {
        if (!string.IsNullOrEmpty(recipeName.text) && ws.chosenRecipe == recipeName.text) ws.chosenRecipe = null;
    }
}
