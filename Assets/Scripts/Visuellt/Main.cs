using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class Main : MonoBehaviour
{
    public Button m_SortedButton, m_RandomSortedButton, m_ReversedSortedButton, m_StartButton;
    public Slider sampleSlider;
    public Text sliderText, exchangeText, timeText;
    public Dropdown sortOption;
    public GameObject bar;

    private float barDistance = 1.5f;
    private int currentIndex = -1;
    private int exchangeCounter = 0;
    private float speed = 0.1f;
    private float startTime = 0f;
    private float executionTime = 0f;
    private bool samplesExist = false;
    private string sampleType = "";
    private List<int> Sortable = new List<int>();
    private List<GameObject> BarList = new List<GameObject>();
    
    void Start()
    {
        Sortable.Clear();
        
        m_SortedButton.onClick.AddListener(SortedButtonClick);
        m_RandomSortedButton.onClick.AddListener(RandomSortedButtonClick);
        m_ReversedSortedButton.onClick.AddListener(ReversedSortedButtonClick);
        m_StartButton.onClick.AddListener(StartButtonClick);

        m_StartButton.interactable = false;
        
        sampleSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        sliderText.text = sampleSlider.value.ToString();
        exchangeText.text = exchangeCounter.ToString();
        timeText.text = executionTime.ToString("0.00");
    }
    
    void Update()
    {
        if (samplesExist == true)
        {
            for(int i = 0; i < sampleSlider.value; i++){
                float xScale = (200 - (sampleSlider.value - 1) * barDistance) / sampleSlider.value;
                float yScale = (sampleSlider.maxValue / sampleSlider.value) * 0.5f;
                BarList[i].transform.localScale = new Vector3(xScale, Sortable[i] * yScale, BarList[i].transform.localScale.z);
                BarList[i].gameObject.GetComponentInChildren<Renderer>().material.color = Color.gray;
                if (i == currentIndex)
                {
                    BarList[i].gameObject.GetComponentInChildren<Renderer>().material.color = Color.magenta;
                }
            }

            timeText.text = executionTime.ToString("0.00");
            exchangeText.text = exchangeCounter.ToString();
        }
    }

    private void DebugList(List<int> aList)
    {
        string temp = "";
        foreach (var i in aList)
        {
            temp += i.ToString() + ", ";
        }
        Debug.Log(temp);
    }
    
    void ClearBars()
    {
        foreach (var cube in BarList)
        {
            Destroy(cube);
        }
        BarList.Clear();
    }

    void DisplayBars()
    {
        ClearBars();
        for (int i = 0; i < sampleSlider.value; i++)
        {
            float x = i * (200 / sampleSlider.value) - 100;
            GameObject cube = Instantiate(bar, new Vector3(x, 0, 0), Quaternion.identity) as GameObject;
            float xScale = (200 - (sampleSlider.value - 1) * barDistance) / sampleSlider.value;
            float yScale = (sampleSlider.maxValue / sampleSlider.value) * 0.5f;
            cube.transform.localScale = new Vector3(xScale, Sortable[i] * yScale, cube.transform.localScale.z);
            BarList.Add(cube);
        }
    }

    void SetButtons(bool onOff)
    {
        m_SortedButton.interactable = onOff;
        m_RandomSortedButton.interactable = onOff;
        m_ReversedSortedButton.interactable = onOff;
        sampleSlider.interactable = onOff;
        sortOption.interactable = onOff;
    }

    public static void addRecord(string sortType, string sampleType, float sampleSize, float time, int exchanges, string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
            {
                file.WriteLine(sortType + "," + sampleType + "," + sampleSize  + "," + time + "," + exchanges);
            }
        }
        catch(Exception ex)
        {
            throw new AggregateException("This program messed up: ", ex);
        }
    }
    
    void StartButtonClick()
    {
        exchangeCounter = 0;
        startTime = Time.time;
        switch (sortOption.value)
        {
            case 0:
                Debug.Log("BUBBLE SORT");
                StartCoroutine(Bubble (Sortable));
                break;
            case 1:
                Debug.Log("BUBBLE SORT OPTIMISED");
                StartCoroutine(Bubble2 (Sortable));
                break;
            case 2:
                Debug.Log("ODD-EVEN SORT");
                StartCoroutine(EvenOdd(Sortable));
                break;
            case 3:
                Debug.Log("COCKTAILSHAKER SORT");
                StartCoroutine(CocktailShaker(Sortable));
                break;
            case 4:
                Debug.Log("INSERTION SORT");
                StartCoroutine(Insertion(Sortable));
                break;
            case 5:
                Debug.Log("BOGO");
                StartCoroutine(Bogo(Sortable));
                break;
            case 6:
                Debug.Log("BOGO2");
                StartCoroutine(Bogo2(Sortable));
                break;
        }
        SetButtons(false);
        m_StartButton.interactable = false;
    }

    void SortedButtonClick()
    {
        currentIndex = -1;
        Sortable.Clear();
        for (int i = 1; i <= sampleSlider.value; i++)
        {
            Sortable.Add(i);
        }

        sampleType = "Sorted";
        DebugList(Sortable);
        m_StartButton.interactable = true;
        DisplayBars();
        samplesExist = true;
    }

    void RandomSortedButtonClick()
    {
        currentIndex = -1;
        Sortable.Clear();
        for (int i = 1; i <= sampleSlider.value; i++)
        {
            Sortable.Add(i);
        }
        for (int i = 1; i <= 1000; i++)
        {
            int i1 = Random.Range(0, (int)sampleSlider.value);
            int i2 = Random.Range(0, (int)sampleSlider.value);
            int temp = Sortable[i1];
            Sortable[i1] = Sortable[i2];
            Sortable[i2] = temp;
        }
        
        sampleType = "Random";
        DebugList(Sortable);
        m_StartButton.interactable = true;
        DisplayBars();
        samplesExist = true;
    }
    
    void ReversedSortedButtonClick()
    {
        currentIndex = -1;
        Sortable.Clear();
        for (int i = 1; i <= sampleSlider.value; i++)
        {
            Sortable.Add(Math.Abs(i - ((int)sampleSlider.value + 1)));
        }
        
        sampleType = "Reversed";
        DebugList(Sortable);
        m_StartButton.interactable = true;
        DisplayBars();
        samplesExist = true;
    }
    
    public void ValueChangeCheck()
    {
        sliderText.text = sampleSlider.value.ToString();
        currentIndex = -1;
        sampleType = "";
        Sortable.Clear();
        ClearBars();
        m_StartButton.interactable = false;
        samplesExist = false;
    }

    IEnumerator Bubble(List<int> SortList){
        int n = SortList.Count;
        bool swapped;
        do {
            swapped = false;
            for (int i = 1; i <= n - 1; i++)
            {
                if (SortList[i - 1] > SortList[i])
                {
                    int t = SortList[i - 1];
                    SortList[i - 1] = SortList[i];
                    SortList[i] = t;

                    exchangeCounter++;
                    swapped = true;
                }
                currentIndex = i;
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
            }
        } while (swapped == true);
        
        addRecord($"BubbleSort", sampleType, sampleSlider.value, executionTime, exchangeCounter, "SortData.csv");
        SetButtons(true);
    }
    
    IEnumerator Bubble2(List<int> SortList){
        int n = SortList.Count;
        bool swapped;
        do {
            swapped = false;
            for (int i = 1; i <= n - 1; i++)
            {
                if (SortList[i - 1] > SortList[i])
                {
                    int t = SortList[i - 1];
                    SortList[i - 1] = SortList[i];
                    SortList[i] = t;

                    exchangeCounter++;
                    swapped = true;
                }
                currentIndex = i;
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
            }
            n = n - 1;
        } while (swapped == true);
        
        addRecord($"BubbleSortOptimised", sampleType, sampleSlider.value, executionTime, exchangeCounter, "SortData.csv");
        SetButtons(true);
    }

    IEnumerator EvenOdd(List<int> SortList)
    {
        bool swapped;
        int n = SortList.Count;

        do
        {
            swapped = false;

            for (int i = 1; i <= n - 2; i = i + 2)
            {
                if (SortList[i] > SortList[i + 1])
                {
                    int temp = SortList[i];
                    SortList[i] = SortList[i + 1];
                    SortList[i + 1] = temp;
                    swapped = true;
                    exchangeCounter++;
                }
                currentIndex = i;
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
            }

            for (int i = 0; i <= n - 2; i = i + 2)
            {
                if (SortList[i] > SortList[i + 1])
                {
                    int temp = SortList[i];
                    SortList[i] = SortList[i + 1];
                    SortList[i + 1] = temp;
                    swapped = true;
                    exchangeCounter++;
                }
                currentIndex = i;
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
            }
        } while (swapped == true);
        addRecord($"EvenOddSort", sampleType, sampleSlider.value, executionTime, exchangeCounter, "SortData.csv");
        SetButtons(true);
    }

    IEnumerator CocktailShaker(List<int> SortList)
    {
        bool swapped;
        int start = 0;
        int end = SortList.Count;

        do {
            swapped = false;

            for (int i = start; i < end - 1; ++i)
            {
                if (SortList[i] > SortList[i + 1])
                {
                    int temp = SortList[i];
                    SortList[i] = SortList[i + 1];
                    SortList[i + 1] = temp;
                    swapped = true;
                    exchangeCounter++;
                }
                currentIndex = i;
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
            }

            //if (swapped == false)
            //    break;

            swapped = false;

            end = end - 1;

            for (int i = end - 1; i >= start; i--)
            {
                if (SortList[i] > SortList[i + 1])
                {
                    int temp = SortList[i];
                    SortList[i] = SortList[i + 1];
                    SortList[i + 1] = temp;
                    swapped = true;
                    exchangeCounter++;
                }
                currentIndex = i;
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
            }

            start = start + 1;
        } while (swapped == true);
        addRecord($"CocktailShakerSort", sampleType, sampleSlider.value, executionTime, exchangeCounter, "SortData.csv");
        SetButtons(true);
    }
    
    IEnumerator Insertion(List<int> SortList){
        for (int i = 0; i < SortList.Count; i++){
            int j = i;
            while (j > 0 && SortList[j - 1] > SortList[j]){
                int temp = SortList[j];
                SortList[j] = SortList[j - 1]; 
                SortList[j - 1] = temp;
                j--;
                
                currentIndex = j; 
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
                exchangeCounter++;
            }
        }
        addRecord($"InsertionSort", sampleType, sampleSlider.value, executionTime, exchangeCounter, "SortData.csv");
        SetButtons(true);
    }

    static bool IsSorted(List<int> SortList)
    {
        for (int i = 0; i < SortList.Count - 1; i++)
        {
            if (SortList[i] > SortList[i + 1])
            {
                return false;
            }
        }

        return true;
    }
    
    IEnumerator Bogo(List<int> SortList){

        while (!IsSorted(SortList))
        {
            System.Random r = new System.Random();

            for (int n = SortList.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);

                int temp = SortList[n];
                SortList[n] = SortList[k];
                SortList[k] = temp;
            
                currentIndex = k; 
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
                exchangeCounter++;
            }
        }
        SetButtons(true);
    }
    
    IEnumerator Bogo2(List<int> SortList){

        int permutate;
        int size = SortList.Count;

        do {
            permutate = 0;
            for (int i = size-1; i > 0; i--) {
                if (SortList[i-1] > SortList[i]) {
                    int j = Random.Range(0, i);
 
                    if (j != i && SortList[j] > SortList[i]) {
                        SortList[i] ^= SortList[j];
                        SortList[j] ^= SortList[i];
                        SortList[i] ^= SortList[j];
                    }
 
                    permutate++;
                }
                currentIndex = i; 
                executionTime = Time.time - startTime;
                yield return new WaitForSeconds(speed);
                exchangeCounter++;
            }
        } while (permutate > 0);
        SetButtons(true);
    }
}
