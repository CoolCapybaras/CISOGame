using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TNRD;
using UnityEngine;
using UnityEngine.UI;

public class FormManager : MonoBehaviour
{
    public static FormManager Instance;
    private Form _activeForm;
    private Stack<Form> _lastForms = new();
    
    [Serializable]
    public class Form
    {
        public string id;
        public GameObject form;
        public OverlayManager.Settings overlaySettings;
    }
    
    public List<Form> forms = new();

    private void Awake()
    {
        Instance = this;

        _activeForm = GetForm("auth");
    }

    private void ChangeForm(string id, bool pushToLastForms)
    {
        if (pushToLastForms)
            _lastForms.Push(_activeForm);
        var newForm = GetForm(id);
        
        // На время анимации отключаем кнопку Назад из оверлея
        OverlayManager.Instance.form.backButton.GetComponent<Button>().interactable = false;
        
        var sequence = DOTween.Sequence();
        sequence.Insert(0, _activeForm.form.GetComponent<CanvasGroup>().DOFade(0, 0.25f).From(1))
            .InsertCallback(0.25f, () => { _activeForm.form.GetComponent<IForm>().OnDisable(); })
            .Insert(0.25f, newForm.form.GetComponent<CanvasGroup>().DOFade(1, 0.25f).From(0))
            .InsertCallback(0.25f, () =>
            {
                _activeForm = newForm;
                _activeForm.form.GetComponent<IForm>().OnActive();
                OverlayManager.Instance.form.backButton.GetComponent<Button>().interactable = true;
                OverlayManager.Instance.ApplySettings(_activeForm.overlaySettings);
            })
            .Play();
    }

    public void ChangeForm(string id)
    {
        ChangeForm(id, true);
    }

    public void ChangeFormToLast() => ChangeForm(_lastForms.Pop().id, false);

    private Form GetForm(string id) => forms.FirstOrDefault(f => f.id == id);
}
