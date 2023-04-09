using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public GameObject carObject;

    public Transform CarsSpawnPosition;

    public List<GameObject> carPool = new List<GameObject>();

    float carMoveSpeed = 10f;

    int currentCarIndex = 0;
    string[] xValues = { "-2", "0", "2" };

private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);
        CreateCarPool();
    }

    private void Start()
    {
        currentCarIndex = 0;
        StartCoroutine(SpawnCarLoop());
    }


    void CreateCarPool()
    {
        for(int i = 0; i < 20; i++)
        {
            var carObj = Instantiate(carObject, this.transform);
            carObj.SetActive(false);
            carPool.Add(carObj);
        }
    }

    IEnumerator SpawnCarLoop()
    {
        spwanCar();
        yield return new WaitForSecondsRealtime(0.7f);
        StartCoroutine(SpawnCarLoop());
    }

    void spwanCar()
    {
        var car = GetCarFromPool();
        currentCarIndex++;

        if(currentCarIndex >= 20)
        {
            currentCarIndex = 0;
        }

        var xValue = Random.Range(0, 3);
        car.transform.position = new Vector3(int.Parse(xValues[xValue]), 0.5f, 20f);
        car.SetActive(true);
        MoveCar(car);

    }

    void MoveCar(GameObject car)
    {
        var seq = DOTween.Sequence();
        car.transform.DOMoveZ(-10f, carMoveSpeed).SetEase(Ease.Linear).OnComplete(() => car.SetActive(false));
    }

    GameObject GetCarFromPool()
    {
        return carPool[currentCarIndex];
    }



}
