using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RunScenario : MonoBehaviour
{


    public ScenarioCreation_UI ScenarioCreation;
    public ScenarioCreation_Function ScenarioLoader;

    public bool IsQueueOn;
    public bool IsScenarioRunning;
    public Dropdown QueueDropdown;
    public Button AddScenarioBtn;
    public Toggle randomiseToggle;
    public Button RunQueueBtn;
    public GameObject ScenarioLookupFeedback;
    public GameObject QueueText;
        public Button ResetQueue;

    public Dropdown ScenarioDropdown;
    public Button SelectButton;
    public int scenarioNumber;
    public int ScenarioIndex;
    public GameObject PlayScenarioMenu;
    public GameObject ViewFieldMenu;
    public GameObject QueueMenu;
    public List<int> ScenarioOnQueue = new List<int>();
    public Button QueueScenarios;
    public Button PlayScenarios;
    public Button MainMenu;
    public GameObject LoadScenarioMenu;
    // Start is called before the first frame update
    void Start()
    {
        AddButtonListeners();
        
    }


    private void AddButtonListeners()
    {//add button option
        SelectButton.onClick.AddListener(SelectScenarioToRun);
        QueueScenarios.onClick.AddListener(QueueScenarioMenu);
        PlayScenarios.onClick.AddListener(LoadScenario);
        MainMenu.onClick.AddListener(OpenMainMenu);
        AddScenarioBtn.onClick.AddListener(SelectScenarioToAdd);
        RunQueueBtn.onClick.AddListener(RunQueue);

        //add dropdown option
        ScenarioDropdown.ClearOptions();
        ScenarioDropdown.AddOptions(ScenarioCreation.scenarios);
        QueueDropdown.ClearOptions();
        QueueDropdown.AddOptions(ScenarioCreation.scenarios
            );


    }

 
    private void OpenMainMenu()
    {
        ViewFieldMenu.SetActive(true);
        GetComponent<Image>().enabled = true;
        PlayScenarioMenu.SetActive(false);
        LoadScenarioMenu.SetActive(false);
        ScenarioCreation.FieldViewMenu.SetActive(false);
        ScenarioCreation.ScenarioMenuObj.SetActive(true);
        ScenarioCreation.SelectScenarioMenuObj.SetActive(false);
    }

    private void QueueScenarioMenu()
    {
        QueueMenu.SetActive(true);
        LoadScenarioMenu.SetActive(false);

    }

    private void LoadScenario()
    {
        PlayScenarioMenu.SetActive(true);
        LoadScenarioMenu.SetActive(false);
        GetComponent<Image>().enabled = false;
        //hide white bg

        
    }

    public void AddScenarioToQueue(int _ScenarioNumber, bool scenarioDetected)
    {
        if (scenarioDetected)
        {
            ScenarioOnQueue.Add(_ScenarioNumber);
            string ScenarioList = "Addded Scenario:";
            string temp="";
           for (int i = 0; i < ScenarioOnQueue.Count; i++)
           {
                int ScenarioTemp = ScenarioOnQueue[i] + 1;
                temp=temp + "\n"+ "Scenario " + ScenarioTemp;
           }

            ScenarioList = ScenarioList + temp;

            QueueText.GetComponent<TextMeshProUGUI>().
              SetText(ScenarioList);
        }
        else
        {
            StartCoroutine(FeedbackCourutine(_ScenarioNumber));

        }


    }

    IEnumerator FeedbackCourutine(int _ScenarioNumber)
    {
        ScenarioLookupFeedback.SetActive(true);
        int scenarioTemp = scenarioNumber + 1;
        ScenarioLookupFeedback.GetComponent<TextMeshProUGUI>().
              SetText("Scenario" + scenarioTemp.ToString() + " is not existed.");
        yield return new WaitForSeconds(3);

        ScenarioLookupFeedback.SetActive(false);

    }

    private void SelectScenarioToAdd()
    {
        scenarioNumber = QueueDropdown.value;
        ScenarioLoader.ScenarioNumber = scenarioNumber;
        ScenarioLoader.ScenarioLookup(ScenarioLoader.ScenarioNumber);
    }
    private void SelectScenarioToRun()
    {
        scenarioNumber = ScenarioDropdown.value;
        ScenarioLoader.ScenarioNumber = scenarioNumber;
        ScenarioLoader.LoadScenario(ScenarioLoader.ScenarioNumber);
        
    }


    public void RunQueue()
    {

        //first check if the current index is bigger than total scenario in the list or not

        if (ScenarioIndex < ScenarioOnQueue.Count)
        {
            QueueMenu.SetActive(false);
            scenarioNumber = ScenarioOnQueue[ScenarioIndex];
            ScenarioLoader.ScenarioNumber = scenarioNumber;
            ScenarioLoader.LoadScenario(ScenarioLoader.ScenarioNumber);
            IsScenarioRunning = true;
            ScenarioIndex++;//add one for the next run
            IsQueueOn = true;
        }
        else
        {
            QueueMenu.SetActive(true);
            
            IsQueueOn = false;
            Debug.Log("All scenario has finished");
        }
    }
  
    // Update is called once per frame
    void Update()
    {
        if (IsScenarioRunning == true)
        {
            //check when it's finished running; and run the queue again
        }
    }
}
