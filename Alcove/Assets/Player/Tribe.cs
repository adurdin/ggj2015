﻿using UnityEngine;
using System.Collections;

public class Tribe : MonoBehaviour, ITowerActionEvents {
	public int count;
	private bool busy;
	private float busyFraction;
	private float busyRemaining;

	// Return true if the tribe is busy
	public bool IsBusy {
		get {
			return busy;
		}
	}

	// Return remaining busy amount as float in range 0..1
	public float BusyFraction {
		get {
			if (busy) {
				return busyFraction;
			} else {
				return 0.0f;
			}
		}
	}

	// Return remaining busy time as whole number of seconds
	public int BusySeconds {
		get {
			if (busy) {
				return (int)Mathf.Ceil(busyRemaining);
			} else {
				return 0;
			}
		}
	}

	public void TowerActionStarted(TowerAction action) {
		busy = true;
	}

	public void TowerActionProgress(TowerAction action, float progress, float secondsRemaining) {
		busyFraction = progress;
		busyRemaining = secondsRemaining;
	}

	public void TowerActionCompleted(TowerAction action) {
		busy = false;
	}
}