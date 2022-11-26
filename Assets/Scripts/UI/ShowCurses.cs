using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ShowCurses : MonoBehaviour
{
    [SerializeField] private Text cursesLabel;

    private void Update()
    {
        List <Curse> curses = Player.Instance.GetCurses();
        curses = curses.OrderBy(w => w.GetType().Name).ToList();

        string text = "";
        string lastType = "";
        foreach (var cur in curses)
        {
            if (cur.GetType().Name == lastType)
            {
                text = text + "+" + cur.stacks.ToString();
            }
            else
            {
                text = text + $" {cur.GetType().Name}: {cur.stacks}";
            }

            lastType = cur.GetType().Name;
        }

        cursesLabel.text = text;
    }
}
