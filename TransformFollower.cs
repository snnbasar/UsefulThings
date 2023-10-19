using UnityEngine;

public class TransformFollower : MonoBehaviour
{

	[SerializeField] private Transform _followObject;

	//public float smoothTime = 0.5f;
	[SerializeField] private float _verticalSmoothTime = 0.1f;
	[SerializeField] private float _horizontalSmoothTime = 0.3f;
	[SerializeField] private float _upSmoothTime = 0.3f;


	[SerializeField] private bool _lockX;
	[SerializeField] private bool _lockY;
	[SerializeField] private bool _lockZ;
	[SerializeField] private Vector3 _lockedPos;

	[SerializeField] private bool _resetOffset;
	[SerializeField] private bool _forceSetPos;





	private Vector3 velocity;

	private Vector3 _startOffset;
	private Vector3 _offset;


    private void Start()
    {
        SetOffset();
    }

    private void SetOffset()
    {
        if (!_followObject) return;
        _offset = transform.position - _followObject.position;
        _startOffset = _offset;
    }

    private void FixedUpdate()
	{
		if (!_followObject) return;

		_offset = _resetOffset ? Vector3.zero : _startOffset;

		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(_lockX ? _lockedPos.x : _followObject.position.x + _offset.x, transform.position.y, transform.position.z), ref velocity, _horizontalSmoothTime);
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, _lockY ? _lockedPos.y : _followObject.position.y + _offset.y, transform.position.z), ref velocity, _horizontalSmoothTime);
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y, _lockZ ? _lockedPos.z : _followObject.position.z + _offset.z), ref velocity, _verticalSmoothTime);

		transform.position = _forceSetPos ? _followObject.position : transform.position;
	}

	public void SetMyFollower(Transform follow)
    {
		_followObject = follow;
		SetOffset();
    }
}