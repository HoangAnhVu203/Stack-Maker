using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public enum Direct  {Forward = 0, Back = 1, Right = 2, Left = 3}
public class Player : MonoBehaviour
{
    public Transform prefBrick;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public LayerMask tileLayer;
    public float rayStart = 1f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float moveSpeed = 8f;
    private List<Transform> playerBricks = new List<Transform>();
    public Transform brickHolder;
    public Transform playerSkin;
    
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if ((GameplayManager.Instance.IsState(GameState.MainMenu) && !isMoving))
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;

        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            CallDirect();
        }
#endif

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    CallDirect();
                    break;
            }
        }
       
        Move();
    }

    public void OnInit()
    {
        isMoving = false;
        Clear();
    }
    
    public void CallDirect()
    {
        

        Vector2 swipe = endTouchPosition - startTouchPosition;
        if (swipe.sqrMagnitude < 10) return;

        swipe.Normalize();

        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            if (swipe.x > 0)
            {
                GetTargetPost(Direct.Right);                
            }
            else
            {
                GetTargetPost(Direct.Left);                
            }
        }
        else
        {
            if (swipe.y > 0)
            {
                GetTargetPost(Direct.Forward);                 
            }
            else
            {
                GetTargetPost(Direct.Back); 
            }
        }

    }

    public void GetTargetPost(Direct dir)
    {
        Vector3 direct = Vector3.zero;

        switch (dir)
        {
            case Direct.Forward:
                direct = Vector3.forward;
                break;

            case Direct.Back:
                direct = Vector3.back;
                break;

            case Direct.Right:
                direct = Vector3.right;
                break;

            case Direct.Left:
                direct = Vector3.left;
                break;
        }

        Vector3 currentPosition = transform.position;
        Vector3 lastHitPosition = currentPosition;
        RaycastHit hit;

        
        while (Physics.Raycast(currentPosition + Vector3.up * 0.5f, direct, out hit, rayStart, tileLayer))
        {
            Debug.DrawRay(currentPosition + Vector3.up * 0.5f, direct * rayStart, Color.red, 1f);
            Debug.Log($"Hit at: {hit.collider.transform.position}");
            lastHitPosition = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
            currentPosition = lastHitPosition;
        }

        targetPosition = lastHitPosition;
        isMoving = true;
        

    }

    public void Move()
    {
        if(!isMoving)
        {
            return;
        }
      
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    public void AddBrick()
    {
        int index = playerBricks.Count;

        Transform playerBrick = Instantiate(prefBrick, brickHolder);
        playerBrick.localPosition = Vector3.down + index * 0.31f * Vector3.up;

        playerBricks.Add(playerBrick);
        //playerSkin.localPosition = playerSkin.localPosition + Vector3.up * 0.31f;
        StartCoroutine(MoveSkinUpSmooth(0.31f));

    }

    public void RemoveBrick()
    {
        int index = playerBricks.Count - 1;
        if(index >= 0)
        {
            Transform playerBrick = playerBricks[index];
            playerBricks.Remove(playerBrick);
            Destroy(playerBrick.gameObject);
            //playerSkin.localPosition = playerSkin.localPosition - Vector3.up * 0.31f;
            StartCoroutine(MoveSkinDownSmooth(0.31f));
        }
    }

    public void Clear()
    {
        for(int i = 0 ; i  <  playerBricks.Count; i++)
        {
            Destroy(playerBricks[i].gameObject);
        }
        playerBricks.Clear();
        playerSkin.localPosition = Vector3.zero;

    }

    private IEnumerator MoveSkinUpSmooth(float distance)
    {
        Vector3 start = playerSkin.localPosition;
        Vector3 end = start + Vector3.up * distance;
        float duration = 0.125f;
        float t = 0;

        while ( t < 1f )
        {
            t += Time.deltaTime / duration;
            playerSkin.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }
        playerSkin.localPosition = end;
    }

    private IEnumerator MoveSkinDownSmooth(float distance)
    {
        Vector3 start = playerSkin.localPosition;
        Vector3 end = start - Vector3.up * distance;

        float duration = 0.125f;
        float t = 0f;

        while( t < 1f)
        {
            t += Time.deltaTime / duration;
            playerSkin.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }

        playerSkin.localPosition = end;
    }
}
