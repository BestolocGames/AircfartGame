using System.Collections;
using CodeBase._Cameras;
using CodeBase._Main;
using CodeBase._Main.Player;
using UnityEngine;

namespace UI
{
	public class StartLevelController : MonoBehaviour
	{
		public bool levelStarted { get; private set; }

		private void Start()
		{
			_autoCam = FindObjectOfType<AutoCam>();
			_pivot = GameObject.Find("Pivot");
			AirplaneUserControl airplaneUserControl = FindObjectOfType<AirplaneUserControl>();
			if (airplaneUserControl == null)
			{
				Debug.LogError("FLIGHT KIT StartLevelSequence: an AeroplaneUserControlcomponent is missing in the scene");
			}
			_airplane = airplaneUserControl.gameObject;
			if (_autoCam == null)
			{
				Debug.LogWarning("Can't find AutoCam component in the scene.");
				enabled = false;
				return;
			}
			_turnSpeed = _autoCam.TurnSpeed;
			_autoCam.TurnSpeed = 0f;
			if (playOnStart)
			{
				StartLevel();
				UIEventsPublisher uieventsPublisher = FindObjectOfType<UIEventsPublisher>();
				if (uieventsPublisher != null)
				{
					uieventsPublisher.PublishPlay();
				}
			}
			else
			{
				UIEventsPublisher.OnPlayEvent += StartLevel;
			}
		}

		private void OnDeactivate()
		{
			UIEventsPublisher.OnPlayEvent -= StartLevel;
		}

		private void Update()
		{
			if (_isTweeningIn)
			{
				_tweenInProgress = Mathf.SmoothStep(0f, 1f, (Time.time - _tweenInStartTime) * 0.14f);
				_autoCam.TurnSpeed = _tweenInProgress * _turnSpeed;
				_pivot.transform.localPosition = Vector3.Lerp(_initPivotPos, cameraPivotFinalPosition, _tweenInProgress);
				if (_tweenInProgress > 0.99f)
				{
					_isTweeningIn = false;
					_autoCam.TurnSpeed = _turnSpeed;
					_pivot.transform.localPosition = cameraPivotFinalPosition;
				}
			}
		}

		public virtual void StartLevel()
		{
			if (this != null)
			{
				StartCoroutine(StartLevelCoroutine());
			}
		}

		private IEnumerator StartLevelCoroutine()
		{
			levelStarted = true;
			PauseController pause = FindObjectOfType<PauseController>();
			if (pause)
			{
				pause.enabled = true;
			}
			yield return new WaitForSeconds((float)((!playOnStart) ? 3 : 0));
			MenuFadeInController gss = FindObjectOfType<MenuFadeInController>();
			if (gss != null)
			{
				GameObject mainMenu = gss.mainMenu;
				if (mainMenu != null)
				{
					mainMenu.SetActive(false);
				}
			}
			_initPivotPos = _pivot.transform.localPosition;
			_isTweeningIn = true;
			_tweenInStartTime = Time.time;
			_autoCam.enabled = true;
			yield return new WaitForSeconds((float)((!playOnStart) ? 2 : 0));
			MonoBehaviour userControl = _airplane.GetComponent<AirplaneUserControl>();
			if (userControl == null)
			{
				Debug.LogError("Can't find AirplaneUserControl component on the airplane.");
				yield break;
			}
			userControl.enabled = true;
			yield return new WaitForSeconds((float)((!playOnStart) ? 2 : 0));
			if (enablePickupGrowingOnStart)
			{
				PickupSphere.growingEnabled = true;
			}
			yield break;
		}

		public bool playOnStart;

		public bool enablePickupGrowingOnStart = true;

		[Space]
		public Vector3 cameraPivotFinalPosition = new Vector3(0f, 10f, -15f);

		private GameObject _airplane;

		private AutoCam _autoCam;

		private float _turnSpeed;

		private GameObject _pivot;

		private Vector3 _initPivotPos;

		private bool _isTweeningIn;

		private float _tweenInProgress;

		private float _tweenInStartTime;
	}
}
