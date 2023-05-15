// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.StartLevelController
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace FlightKit
{
	public class StartLevelController : MonoBehaviour
	{
		public bool levelStarted { get; private set; }

		private void Start()
		{
			this._autoCam = UnityEngine.Object.FindObjectOfType<AutoCam>();
			this._pivot = GameObject.Find("Pivot");
			AirplaneUserControl airplaneUserControl = UnityEngine.Object.FindObjectOfType<AirplaneUserControl>();
			if (airplaneUserControl == null)
			{
				UnityEngine.Debug.LogError("FLIGHT KIT StartLevelSequence: an AeroplaneUserControlcomponent is missing in the scene");
			}
			this._airplane = airplaneUserControl.gameObject;
			if (this._autoCam == null)
			{
				UnityEngine.Debug.LogWarning("Can't find AutoCam component in the scene.");
				base.enabled = false;
				return;
			}
			this._turnSpeed = this._autoCam.TurnSpeed;
			this._autoCam.TurnSpeed = 0f;
			if (this.playOnStart)
			{
				this.StartLevel();
				UIEventsPublisher uieventsPublisher = UnityEngine.Object.FindObjectOfType<UIEventsPublisher>();
				if (uieventsPublisher != null)
				{
					uieventsPublisher.PublishPlay();
				}
			}
			else
			{
				UIEventsPublisher.OnPlayEvent += this.StartLevel;
			}
		}

		private void OnDeactivate()
		{
			UIEventsPublisher.OnPlayEvent -= this.StartLevel;
		}

		private void Update()
		{
			if (this._isTweeningIn)
			{
				this._tweenInProgress = Mathf.SmoothStep(0f, 1f, (Time.time - this._tweenInStartTime) * 0.14f);
				this._autoCam.TurnSpeed = this._tweenInProgress * this._turnSpeed;
				this._pivot.transform.localPosition = Vector3.Lerp(this._initPivotPos, this.cameraPivotFinalPosition, this._tweenInProgress);
				if (this._tweenInProgress > 0.99f)
				{
					this._isTweeningIn = false;
					this._autoCam.TurnSpeed = this._turnSpeed;
					this._pivot.transform.localPosition = this.cameraPivotFinalPosition;
				}
			}
		}

		public virtual void StartLevel()
		{
			if (this != null)
			{
				base.StartCoroutine(this.StartLevelCoroutine());
			}
		}

		private IEnumerator StartLevelCoroutine()
		{
			this.levelStarted = true;
			PauseController pause = UnityEngine.Object.FindObjectOfType<PauseController>();
			if (pause)
			{
				pause.enabled = true;
			}
			yield return new WaitForSeconds((float)((!this.playOnStart) ? 3 : 0));
			MenuFadeInController gss = UnityEngine.Object.FindObjectOfType<MenuFadeInController>();
			if (gss != null)
			{
				GameObject mainMenu = gss.mainMenu;
				if (mainMenu != null)
				{
					mainMenu.SetActive(false);
				}
			}
			this._initPivotPos = this._pivot.transform.localPosition;
			this._isTweeningIn = true;
			this._tweenInStartTime = Time.time;
			this._autoCam.enabled = true;
			yield return new WaitForSeconds((float)((!this.playOnStart) ? 2 : 0));
			MonoBehaviour userControl = this._airplane.GetComponent<AirplaneUserControl>();
			if (userControl == null)
			{
				UnityEngine.Debug.LogError("Can't find AirplaneUserControl component on the airplane.");
				yield break;
			}
			userControl.enabled = true;
			yield return new WaitForSeconds((float)((!this.playOnStart) ? 2 : 0));
			if (this.enablePickupGrowingOnStart)
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
