using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
using System.Linq;

public class CstmzCategoryDropdown : MonoBehaviour{
    [SerializeField]List<string> skip = new();
    TMP_Dropdown dd;
    void Start(){
        dd=GetComponent<TMP_Dropdown>();
        
        var categoryNames = CustomizationInventory.instance._CstmzCategoryNames;

        var options = categoryNames
            .Where(name => skip.Count == 0 || !skip.Any(skipName => name.Equals(skipName, StringComparison.OrdinalIgnoreCase)))
            .Select(name => new OptionData(name, dd.itemImage.sprite, Color.white))
            .ToList();

        dd.ClearOptions();
        dd.AddOptions(options);
        SetValueFromSelected();
    }
    
    public void SetCategory(){CustomizationInventory.instance.ChangeCategory(dd.options[dd.value].text);}
    public void SetValue(string str){dd.value=dd.options.FindIndex(d=>d.text.Equals(str, StringComparison.OrdinalIgnoreCase));}
    public void SetValueFromSelected()
    {
        var selectedName = CustomizationInventory.instance._CstmzCategoryNames[(int)CustomizationInventory.instance.categorySelected];
        dd.value = dd.options.FindIndex(d => d.text.Equals(selectedName, StringComparison.OrdinalIgnoreCase));
    }
}