using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgrade : MonoBehaviour
{
    [SerializeField] private Image _xpBar;
    [SerializeField] private TextMeshProUGUI _xpBarText;
    [SerializeField] private RectTransform _upgradePanelTransform;
    [SerializeField] private float _upgradePanelAppearTime = 0.25f;
    [SerializeField] private Transform _upgradeHolder;
    private GameObject[] _currentUpgradeButtons;
    private float _currentXPAmount = 0;
    private float _levelUpXPAmount;
    private int _currentLevel = 0;

    private bool _isInUpgrade = false;

    private PlayerMovement _pm;
    private PlayerHealth _ph;

    private int[] _amountOfXPPerLevel =
        {100,
         200,
         400,
         800,
         1600,
         3200};

    private void Start()
    {
        _pm = PlayerManager.Instance.PlayerMovement;
        _ph = PlayerManager.Instance.PlayerHealth;
        _upgradeLevels = new int[_upgradeButtons.Count];
        _currentUpgradeButtons = new GameObject[4];
        _levelUpXPAmount = _amountOfXPPerLevel[0];
    }

    public void CollectExperience(float amount)
    {
        _currentXPAmount += amount;

        //if leveled up
        if(_currentXPAmount >= _levelUpXPAmount)
        {
            //fill bar
            _xpBar.fillAmount = 1;
            if(!_isInUpgrade) OpenUpgradeScreen();
            return;
        }

        //xp added to bar
        _xpBar.fillAmount = _currentXPAmount / _levelUpXPAmount;
    }

    public void OpenUpgradeScreen()
    {
        _isInUpgrade = true;    

        //slide panel in
        _upgradePanelTransform.anchoredPosition = new Vector2(0, -1000);
        _upgradePanelTransform.DOAnchorPos(Vector2.zero, _upgradePanelAppearTime, false).SetEase(Ease.InCubic);

        //set panel up
        for(int i = 0; i < 4; i++)
        {
            int randomIndexInUpgradeButtons = Random.Range(0, _upgradeButtons.Count);
            GameObject randUpgrade = _upgradeButtons[randomIndexInUpgradeButtons];
            GameObject newButton = Instantiate(randUpgrade, _upgradeHolder);
            var upgrade = newButton.GetComponent<Upgrades>();
            upgrade.Init(this);

            _currentUpgradeButtons[i] = newButton;
            //gets child level text component
            var newButLevel = newButton.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            if (_upgradeLevels[randomIndexInUpgradeButtons] == 0)
            {
                //new upgrade
                newButLevel.text = "new!";
            }
            else if (_upgradeLevels[randomIndexInUpgradeButtons] > 0)
            {
                //normal upgrade 
                newButLevel.text = "LVL: " + _upgradeLevels[randomIndexInUpgradeButtons];
            }
        }
        _currentLevel++;
    }

    private void CloseOutOfUpgradeScreen()
    {
        //slide panel out
        _upgradePanelTransform.anchoredPosition = new Vector2(0, 0);
        _upgradePanelTransform.DOAnchorPos(new Vector2(0, -1000), _upgradePanelAppearTime, false).SetEase(Ease.InCubic);

        //clear upgrade buttons 
        for(int i = 0; i < 4; i++)
        {
            if (_currentUpgradeButtons[i] != null)
            {
                Destroy(_currentUpgradeButtons[i]);
                _currentUpgradeButtons[i] = null;
            }  
        }

        //reset xp
        _currentXPAmount -= _levelUpXPAmount;
        _levelUpXPAmount = _amountOfXPPerLevel[_currentLevel];

        //update ui
        _xpBar.fillAmount = _currentXPAmount / _levelUpXPAmount;
        _xpBarText.text = "LVL: " + _currentLevel.ToString();

        //check if upgrade again right away
        if(_currentXPAmount >= _levelUpXPAmount)
        {
            Invoke(nameof(OpenUpgradeScreen), _upgradePanelAppearTime);
        }
        _isInUpgrade = false;
        Debug.Log(_levelUpXPAmount);
    }

    private IEnumerator UpgradeScreen()
    {
        yield return null;
    }

    #region Upgrades

    [SerializeField] private List<GameObject> _upgradeButtons;
    private int[] _upgradeLevels;

    //upgrade 0
    [SerializeField] private float _damageBoost = 5f;
    public void DamageBoost()
    {
        _upgradeLevels[0]++;
        _pm.BaseDamage += _damageBoost;
        CloseOutOfUpgradeScreen();
    }

    //upgrade 1
    [SerializeField] private float _speedBoost = 1.0f;
    public void SpeedBoost()
    {
        _upgradeLevels[1]++;
        _pm.PlayerSpeed += _speedBoost;
        CloseOutOfUpgradeScreen();
    }

    //upgrade 2
    [SerializeField] private float _healthBoost = 5.0f;
    public void HealthBoost()
    {
        _upgradeLevels[2]++;
        _ph.MaxPlayerHealth += _healthBoost;
        _ph.CurrentPlayerHealth += _healthBoost;
        CloseOutOfUpgradeScreen();
    }

    //upgrade 3
    [SerializeField] private float _armorBoost = 1.0f;
    public void ArmorBoost()
    {
        _upgradeLevels[3]++;
        _ph.Armor += _armorBoost;
        CloseOutOfUpgradeScreen();
    }

    #endregion
}
