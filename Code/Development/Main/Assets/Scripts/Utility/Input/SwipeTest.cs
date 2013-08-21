using UnityEngine;
using System.Collections;
 
public class SwipeTest : MonoBehaviour {
 
    // Values to set:
    public float comfortZone = 70.0f;
    public float minSwipeDist = 14.0f;
    public float maxSwipeTime = 0.5f;
 
    private float startTime;
    private Vector2 startPos;
    private bool couldBeSwipe;
    
    public enum SwipeDirection 
	{
        None,
        Up,
        Down
    }
    
    public SwipeDirection lastSwipe = SwipeDirection.None;
    public float lastSwipeTime;
    
    void  Update() 
    {
        if (Input.touchCount > 0) 
        {
					Debug.Log( "********************* FOUND TOUCH *****************" );
            Touch touch = Input.touches[0];
        
            switch (touch.phase) 
            {
                case TouchPhase.Began:
				
					Debug.Log( "********************* BEGAN TOUCH *****************" );
				
                    lastSwipe = SwipeTest.SwipeDirection.None;
                    lastSwipeTime = 0;
				
                    couldBeSwipe = true;
				
                    startPos = touch.position;
                    startTime = Time.time;
                    break;
                
                case TouchPhase.Moved:
				
					Debug.Log( "********************* MOVED TOUCH *****************" );
                   if (Mathf.Abs(touch.position.x - startPos.x) > comfortZone) 
                    {
                        Debug.Log("Not a swipe. Swipe strayed " + (int)Mathf.Abs(touch.position.x - startPos.x) + 
                                  "px which is " + (int)(Mathf.Abs(touch.position.x - startPos.x) - comfortZone) + 
                                  "px outside the comfort zone.");
                        couldBeSwipe = false;
                    }
                    break;
				
                case TouchPhase.Ended:
					Debug.Log( "********************* ENDED TOUCH *****************" );
				
                    if (couldBeSwipe)
                    {
                        float swipeTime = Time.time - startTime;
                        float swipeDist = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                    
                        if ((swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist)) 
                        {
                            // It's a swiiiiiiiiiiiipe!
                            float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                        
                            // If the swipe direction is positive, it was an upward swipe.
                            // If the swipe direction is negative, it was a downward swipe.
                            if (swipeValue > 0)
                                lastSwipe = SwipeTest.SwipeDirection.Up;
                            else if (swipeValue < 0)
                                lastSwipe = SwipeTest.SwipeDirection.Down;
                        
                            // Set the time the last swipe occured, useful for other scripts to check:
                            lastSwipeTime = Time.time;
                            Debug.Log("Found a swipe!  Direction: " + lastSwipe);
                        }
                    }
                    break;
            }
        }
    }
}
