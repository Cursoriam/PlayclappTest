using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private InputField CreationCoolDownInputField;
    [SerializeField] private InputField CubeVelocityInputField;
    [SerializeField] private InputField DestructionDistanceInputField;
    [SerializeField] private GameObject ui;
    private float _coolDownCreationTime;
    private float _velocity;
    private float _destructionDistance;

    public void StartGame()
    {
        GetValuesFromInput();
        Destroy(ui);
        CreateCubes();
    }

    private void CreateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = spawnPoint;
        cube.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f),
            Random.Range(0f, 1f));
        StartCoroutine(MoveAndDestroyAfter(cube));
    }

    private void CreateCubes()
    {
        InvokeRepeating(nameof(CreateCube),0.0f, _coolDownCreationTime);
    }

    private void GetValuesFromInput()
    {
        float.TryParse(CreationCoolDownInputField.text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out _coolDownCreationTime);
        float.TryParse(CubeVelocityInputField.text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out _velocity);
        float.TryParse(CubeVelocityInputField.text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out _destructionDistance);
        _coolDownCreationTime = ValidateFloat(_coolDownCreationTime);
        _velocity = ValidateFloat(_velocity);
        _destructionDistance = ValidateFloat(_destructionDistance);
    }

    private float ValidateFloat(float value)
    {
        if (value < Constants.MinValue)
            return Constants.MinValue;

        if (value > Constants.MaxValue)
            return Constants.MaxValue;

        return value;
    }

    private IEnumerator MoveAndDestroyAfter(GameObject go)
    {
        var currentPosition = go.transform.position;
        var coordinate = new Vector3(currentPosition.x + _destructionDistance, 0.0f,
            currentPosition.z + _destructionDistance);
        var elapsedTime = 0.0f;
        var movementTime = _destructionDistance / _velocity;
        while (elapsedTime < movementTime)
        {
            go.transform.position = Vector3.Lerp(currentPosition, coordinate, elapsedTime / movementTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        go.transform.position = coordinate;
        Destroy(go);
        yield return null;
    }
}
