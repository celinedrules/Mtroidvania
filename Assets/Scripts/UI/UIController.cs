using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Sprite energyTankFull;
    [SerializeField] private Sprite energyTankEmpty;
    [SerializeField] private GameObject energyTankPrefab;
    [SerializeField] private GameObject energyTankContainer;
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 2.0f;

    private string _energyString;
    private float _fullTanks;
    private readonly List<GameObject> _energyTanks = new();
    private bool _fadingOut;
    private bool _fadingIn;

    private void Update()
    {
        if (_fadingOut)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1.0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a >= 1.0f)
                _fadingOut = false;
        }
        else if (_fadingIn)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0.0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a <= 0.0f)
                _fadingIn = false;
        }
    }

    public void SetupPlayerHealth(int maxHealth)
    {
        _energyString = maxHealth.ToString();

        _fullTanks = GetFullTanks(maxHealth);

        for (var i = 0; i < _fullTanks; i++)
            _energyTanks.Add(Instantiate(energyTankPrefab, energyTankContainer.transform));
        
        _energyString = (Mathf.Max(0, maxHealth) - _fullTanks * 100).ToString();
        
        if (_energyString.Length == 1)
            _energyString = "0" + _energyString;

        healthText.text = "Energy: " + _energyString;
    }
    
    public void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        _fullTanks = GetFullTanks(currentHealth);

        int numEmptyTanks = _energyTanks.Count - (int)_fullTanks;

        foreach (GameObject tank in _energyTanks)
            tank.GetComponent<Image>().sprite = energyTankFull;

        for (int i = 0; i < numEmptyTanks; i++)
            _energyTanks[_energyTanks.Count - 1 - i].GetComponent<Image>().sprite = energyTankEmpty;
        
        _energyString = (Mathf.Max(0, currentHealth) - _fullTanks * 100).ToString();
        
        if (_energyString.Length == 1)
            _energyString = "0" + _energyString;

        healthText.text = "Energy: " + _energyString;
    }

    public void AddHealthTank(int currentHealth, int maxHealth)
    {
        _energyTanks.Add(Instantiate(energyTankPrefab, energyTankContainer.transform));
        UpdatePlayerHealth(currentHealth, maxHealth);
    }
    
    private static float GetFullTanks(int health) => Mathf.Floor(Mathf.Max(0, health) / 100f);

    public void FadeInOut(bool fadeout)
    {
        if (fadeout)
        {
            _fadingOut = true;
            _fadingIn = false;
        }
        else
        {
            _fadingIn = true;
            _fadingOut = false;
        }
    }
}
