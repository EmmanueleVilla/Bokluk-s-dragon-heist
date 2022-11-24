using System.Collections;
using UnityEngine;

public class TrampolineManager : MonoBehaviour {
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject up;
    public float trampolinePower;

    private void Awake() {
        up.SetActive(false);
    }

    public void Trigger() {
        down.SetActive(false);
        up.SetActive(true);
        StartCoroutine(ResetTrigger());
    }

    private IEnumerator ResetTrigger() {
        yield return new WaitForSeconds(2.0f);
        down.SetActive(true);
        up.SetActive(false);
    }
}