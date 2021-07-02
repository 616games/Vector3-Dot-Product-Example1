using UnityEngine;
using Random = UnityEngine.Random;

public class CalculateDotProduct : MonoBehaviour
{
    #region --Fields / Properties--
    
    /// <summary>
    /// First unscaled vector to be drawn.
    /// </summary>
    private Vector3 _vectorA;
    
    /// <summary>
    /// Second unscaled vector to be drawn.
    /// </summary>
    private Vector3 _vectorB;
    
    /// <summary>
    /// First scaled vector to be drawn.
    /// </summary>
    private Vector3 _scaledVectorA;
    
    /// <summary>
    /// Second scaled vector to be drawn.
    /// </summary>
    private Vector3 _scaledVectorB;
    
    /// <summary>
    /// First scalar to be used on first vector.
    /// </summary>
    private int _scalarA;
    
    /// <summary>
    /// Second scalar to be used on second vector.
    /// </summary>
    private int _scalarB;
    
    /// <summary>
    /// Used to switch between displaying scaled and unscaled vector sets.
    /// </summary>
    private float _switchTime = 5f;
    
    /// <summary>
    /// Tracks the _switchTime.
    /// </summary>
    private float _switchTimer;
    
    /// <summary>
    /// Determines which set of vectors should be drawn.
    /// </summary>
    private bool _scaled;

    /// <summary>
    /// Stores the unscaled dot product.
    /// </summary>
    private float _unscaledDotProduct;
    
    /// <summary>
    /// Stores the scaled dot product.
    /// </summary>
    private float _scaledDotProduct;

    /// <summary>
    /// Stores the first side that is not the hypotenuse.
    /// </summary>
    private float _side1;

    /// <summary>
    /// Stores the second side that is not the hypotenuse.
    /// </summary>
    private float _side2;

    #endregion
    
    #region --Unity Specific Methods--

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        CheckTimer();
    }
    
    #endregion
    
    #region --Custom Methods--
    
    /// <summary>
    /// Initializes variables and caches components.
    /// </summary>
    private void Init()
    {
        _scalarA = Random.Range(2, 11);
        _scalarB = Random.Range(2, 11);
        
        _vectorA = new Vector3(Random.Range(Random.Range(-20f, -5f), Random.Range(5f, 20f)), Random.Range(Random.Range(-20f, -5f), Random.Range(5f, 20f)), 
                               Random.Range(Random.Range(-5f, -20f), Random.Range(5f, 20f)));
        _vectorB = new Vector3(Random.Range(Random.Range(-20f, -5f), Random.Range(5f, 20f)), Random.Range(Random.Range(-20f, -5f), Random.Range(5f, 20f)), 
                               Random.Range(Random.Range(-5f, -20f), Random.Range(5f, 20f)));

        CheckDotProduct();
    }

    /// <summary>
    /// Keeps track of the _switchTime for graphing the unscaled and scaled vector sets.
    /// </summary>
    private void CheckTimer()
    {
        _switchTimer += Time.deltaTime;
        if (_switchTimer > _switchTime)
        {
            _switchTimer = 0.0f;
            _scaled = !_scaled;
        }
        
        if (_scaled)
        {
            DrawVectors(Vector3.zero, _scaledVectorA, Color.yellow);
            DrawVectors(Vector3.zero, _scaledVectorB, Color.magenta);
        }
        else
        {
            DrawVectors(Vector3.zero, _vectorA, Color.cyan);
            DrawVectors(Vector3.zero, _vectorB, Color.red);
        }
    }

    /// <summary>
    /// Performs the dot product for both unscaled and scaled vector sets and outputs the findings to the console.
    /// </summary>
    private void CheckDotProduct()
    {
        _scaledVectorA = _vectorA * _scalarA;
        _scaledVectorB = _vectorB * _scalarB;

        _unscaledDotProduct = DotProductSimple(_vectorA, _vectorB);
        _scaledDotProduct = DotProductSimple(_scaledVectorA, _scaledVectorB);

        Debug.Log("Unscaled dot product: " + _unscaledDotProduct);
        Debug.Log("Scaled dot product: " + _scaledDotProduct);
        
        if (_unscaledDotProduct > 0)
        {
            Debug.Log("VectorA and VectorB point in the same direction and the angle between them is less than 90 degrees.");
        }
        else if (_unscaledDotProduct < 0)
        {
            Debug.Log("VectorA and VectorB point in opposite directions and the angle between them is greater than 90 degrees.");
        }
        else if (_unscaledDotProduct < 0 && _unscaledDotProduct > 0)
        {
            Debug.Log("VectorA and VectorB are orthogonal to each other.");
        }
        else if(Mathf.Approximately(_unscaledDotProduct, (Magnitude(_vectorA) * Magnitude(_vectorB))))
        {
            Debug.Log("VectorA and VectorB are collinear and the angle between them is 0 degrees.");
        }
        else if (Mathf.Approximately(_unscaledDotProduct, -(Magnitude(_vectorA) * Magnitude(_vectorB))))
        {
            Debug.Log("VectorA and VectorB are collinear and the angle between them is 180 degrees.");
        }
    }
    
    /// <summary>
    /// Finds the angle degrees between two provided vectors. 
    /// </summary>
    private float FindAngleBetweenVectors(Vector3 _vector1, Vector3 _vector2)
    {
        return Mathf.Acos(_unscaledDotProduct / (Magnitude(_vector1) * Magnitude(_vector2)));
    }

    /// <summary>
    /// Returns the magnitude of the provided vector.
    /// </summary>
    private float Magnitude(Vector3 _vector)
    {
        float _absX = Mathf.Abs(_vector.x);
        float _absY = Mathf.Abs(_vector.y);
        float _absZ = Mathf.Abs(_vector.z);
        
        if (_absX > _absY && _absX > _absZ)
        {
            _side1 = _absY;
            _side2 = _absZ;
        }
        else if (_absY > _absX && _absY > _absZ)
        {
            _side1 = _absX;
            _side2 = _absZ;
        }
        else if (_absZ > _absX && _absZ > _absY)
        {
            _side1 = _absX;
            _side2 = _absY;
        }
        else
        {
            return 1;
        }

        return Mathf.Sqrt((_side1 * _side1) + (_side2 * _side2));
    }

    /// <summary>
    /// Calculates the dot product using the geometric equation.
    /// </summary>
    private float DotProductComplex(Vector3 _vector1, Vector3 _vector2)
    {
        float _angle = FindAngleBetweenVectors(_vector1, _vector2);
        float _magnitudeV1 = Magnitude(_vector1);
        float _magnitudeV2 = Magnitude(_vector2);
        float _cosTheta = Mathf.Cos(_angle * Mathf.Deg2Rad);
        return _magnitudeV1 * _magnitudeV2 * _cosTheta;
    }

    /// <summary>
    /// Calculates the dot product using coordinates.
    /// </summary>
    private float DotProductSimple(Vector3 _vector1, Vector3 _vector2)
    {
        return (_vector1.x * _vector2.x) + (_vector1.y * _vector2.y) + (_vector1.z * _vector2.z);
    }

    /// <summary>
    /// Draws the vectors in the Scene and Game view.
    /// </summary>
    private void DrawVectors(Vector3 _from, Vector3 _to, Color _color)
    {
        Debug.DrawRay(_from, _to, _color);
    }
    
    #endregion
    
}
