using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

// runs in the dartboard game object.
// its main purpose is to calculate the score and throws to end the game.
// it also updates the ui to the player knows his progress.
public class Score : MonoBehaviour
{
    // game starts with 301 points (couls also be 501)
    private int _score = 301;
    public TextMeshProUGUI ScoreTxt;
    public TextMeshProUGUI LastShot;
    public TextMeshProUGUI Multi;
    private string _multi;
    private float _scale;
    private float _points = 0;

    // when dart hits dartboard
    private void OnCollisionEnter(Collision collision)
    {
        // from contact point get radius and angle
        Vector3 contactPoint = collision.GetContact(0).point;
        Vector3 direction = contactPoint - transform.position;
        float radius = Vector3.Distance(transform.position, contactPoint);

        Vector3 zeroAngleAxis = transform.right;
        float angle = - Vector3.SignedAngle(zeroAngleAxis, direction, transform.up);

        UpdateScore(radius,angle);
        Debug.Log($"radius: {radius}, angle: {angle}");
    }
    
    private void Start()
    {
        ScoreTxt.text = $"Score: {_score:F0}";
        // when changing the dartoboard's scale, points will not be affected
        Vector3 scale = transform.localScale;
        _scale = scale.x * 1.9f;
    }

    // gets score according to angle and radius.
    // also updates ui.
    private void UpdateScore(float radius, float angle)
    {
        _multi = "";
        _points = 0;
        if (radius < 0.01616 * _scale)
        {
            _multi = "Center of Dartboard!";
            _score -= 50;
            _points = 50;
        }
        else if (radius < 0.041 * _scale)
        {
            _score -= 25;
            _points = 25;
        }
        else if (radius < 0.246 * _scale)
        {
            PointsFromAngle(angle);
            _score -= (int)_points;
        }
        else if (radius < 0.264 * _scale)
        {
            _multi = "Triple Points!";
            PointsFromAngle(angle);
            _score -= (int)(_points * 3);
        }
        else if (radius < 0.396 * _scale)
        {
            PointsFromAngle(angle);
            _score -= (int)_points;
        }
        else if (radius < 0.42 * _scale)
        {
            _multi = "Double Points";
            PointsFromAngle(angle);
            _score -= (int)(_points * 2);
        }

        if (_score < 0)
        {
            GameOver();
        }

        ScoreTxt.text = $"Score: {_score:F0}";
        LastShot.text = $"Last Shot: {_points:F0}";
        Multi.text = $"{_multi}";
    }

    // when points<0, game is over.
    // update singleton and change scene to gameover scene.
    private void GameOver()
    {
        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        SavedData.Instance.UpdateTime(time);
        SceneManager.LoadScene(2);
    }

    // get points according to angle (called in UpdateScore)
    private void PointsFromAngle(float angle)
    {
        if (angle < -180 + 9)
        {
            _points = 11;
        }
        else if (angle < -180 + 9 * 3)
        {
            _points = 8;
        }
        else if (angle < -180 + 9 * 5)
        {
            _points = 16;
        }
        else if (angle < -180 + 9 * 7)
        {
            _points = 7;
        }
        else if (angle < -180 + 9 * 9)
        {
            _points = 19;
        }
        else if (angle < -180 + 9 * 11)
        {
            _points = 3;
        }
        else if (angle < -180 + 9 * 13)
        {
            _points = 17;
        }
        else if (angle < -180 + 9 * 15)
        {
            _points = 2;
        }
        else if (angle < -180 + 9 * 17)
        {
            _points = 15;
        }
        else if (angle < -180 + 9 * 19)
        {
            _points = 10;
        }
        else if (angle < 9)
        {
            _points = 6;
        }
        else if (angle < 9 * 3)
        {
            _points = 13;
        }
        else if (angle < 9 * 5)
        {
            _points = 4;
        }
        else if (angle < 9 * 7)
        {
            _points = 18;
        }
        else if (angle < 9 * 9)
        {
            _points = 1;
        }
        else if (angle < 9 * 11)
        {
            _points = 20;
        }
        else if (angle < 9 * 13)
        {
            _points = 5;
        }
        else if (angle < 9 * 15)
        {
            _points = 12;
        }
        else if (angle < 9 * 17)
        {
            _points = 9;
        }
        else if (angle < 9 * 19)
        {
            _points = 14;
        }
        else
        {
            _points = 11;
        }
    }
}
