using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class playerScript : MonoBehaviour
{
    public static playerScript _instance;

    Transform carToJump;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void FindClosestCar(SwipeDirection swipeDirection, PlayerLane playerLane)
    {
        float minnimumDistance = 5;
        foreach( var car in GameManager._instance.carPool)
        {
            if (car.activeInHierarchy)
            {
                if (car.transform.position.z > gameObject.transform.position.z)
                {
                    switch(swipeDirection)
                    {
                        case SwipeDirection.Right:
                            {
                                if (car.transform.position.x > gameObject.transform.position.x )
                                {
                                    // Jump to right car;
                                    float distance = Vector3.Distance(gameObject.transform.position, car.transform.position);

                                    if (distance < minnimumDistance)
                                    {
                                        minnimumDistance = distance;
                                        carToJump = car.transform;
                                    }
                                }
                            }
                            break;
                        case SwipeDirection.Left:
                            {
                                if (car.transform.position.x < gameObject.transform.position.x )
                                {
                                    // jump to left car;
                                    float distance = Vector3.Distance(gameObject.transform.position, car.transform.position);

                                    if (distance < minnimumDistance)
                                    {
                                        minnimumDistance = distance;
                                        carToJump = car.transform;
                                    }
                                }
                            }
                            break;
                        case SwipeDirection.Front:
                            {

                                if (car.transform.position.z > gameObject.transform.position.z && car.transform.position.x == gameObject.transform.position.x && gameObject.transform.parent != car.transform )
                                {
                                    minnimumDistance = 3f;
                                    // jump to Front car;
                                    float distance = Vector3.Distance(gameObject.transform.position, car.transform.position);

                                    if (distance < minnimumDistance)
                                    {
                                        minnimumDistance = distance;
                                        carToJump = car.transform;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
        if(carToJump != null)
        {
            JumpToCar(carToJump);
        }
    }

    void JumpToCar(Transform newCar)
    {
        gameObject.transform.DOJump(new Vector3(newCar.position.x, gameObject.transform.position.y, (newCar.position.z - 0.5f)), 1f, 1, 0.1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => 
            { 
                gameObject.transform.SetParent(newCar.transform);
            });
        

        //gameObject.transform.position = new Vector3(newCar.position.x, gameObject.transform.position.y, newCar.position.z);
        carToJump = null;
    }

    
}