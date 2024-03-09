using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SavingWrapper : MonoBehaviour
{
    private const string DefaultSaveFile = "save";

    private PlayerInputActions _saveControls;
    private InputAction _save;
    private InputAction _load;
    private InputAction _delete;
    
    public string SaveFile => DefaultSaveFile;

    public void Save(InputAction.CallbackContext ctx) => JsonSavingSystem.Instance.Save(DefaultSaveFile);
    public void Load(InputAction.CallbackContext ctx) => JsonSavingSystem.Instance.Load(DefaultSaveFile);
    public void Delete(InputAction.CallbackContext ctx) => GetComponent<JsonSavingSystem>().Delete(SaveFile);

    private void Awake()
    {
        _saveControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _save = _saveControls.QuickActions.SaveGame;
        _save.Enable();
        _save.performed += Save;
        
        _load = _saveControls.QuickActions.LoadGame;
        _load.Enable();
        _load.performed += Load;
        
        _delete = _saveControls.QuickActions.DeleteSaveFile;
        _delete.Enable();
        _delete.performed += Delete;
    }

    private void OnDisable()
    {
        _save.Disable();
        _load.Disable();
        _delete.Disable();
    }
}
