﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour, ITowerSegmentCallback {
	private List<TowerSegment> segments;
	public int m_cursorPosition;
	private int m_selectedPrefabIndex;
	private Player m_owningPlayer;

	public GameObject m_selector;
	public PrefabSelector m_prefabSelector;

	public TowerSegment m_emptyTowerSegmentPrefab;
	public TowerSegment m_constructionTowerSegmentPrefab;
	public TowerSegment m_staticTowerSegmentPrefab;
	public TowerSegment m_cannonTowerSegmentPrefab;
	public TowerSegment m_baseTowerSegmentPrefab;

	public List<TowerSegment> m_constructableTowerSegments;
	
	void Awake () {
		segments = new List<TowerSegment> ();
		m_cursorPosition = 0;
		AddTowerSegment(m_baseTowerSegmentPrefab);
		AddTowerSegment(m_emptyTowerSegmentPrefab);

		m_constructableTowerSegments = new List<TowerSegment>();
		m_constructableTowerSegments.Add(m_staticTowerSegmentPrefab);
		m_constructableTowerSegments.Add(m_cannonTowerSegmentPrefab);
	}

	void Start() {
		m_prefabSelector = GetComponentInChildren<PrefabSelector>();
		foreach (TowerSegment segment in m_constructableTowerSegments) {
			SpriteRenderer spriteRenderer = segment.GetComponent<SpriteRenderer>();
			m_prefabSelector.AddSelection(spriteRenderer.sprite);
		}
	}

	public void Update() {
		ShowOrHidePrefabSelector();
	}

	public TowerSegment AddTowerSegment(TowerSegment towerSegmentPrefab) {
		TowerSegment newSegment = (TowerSegment)Instantiate(towerSegmentPrefab);
		newSegment.transform.parent = transform;
		newSegment.transform.localPosition = Vector3.up * (float)segments.Count * TowerSegment.HEIGHT;
		segments.Add(newSegment);
		return newSegment;
	}

	public TowerSegment SwapSegment(TowerSegment oldSegment, TowerSegment prefab) {
		int index = segments.IndexOf(oldSegment);
		TowerSegment newSegment = (TowerSegment)Instantiate(prefab);
		newSegment.transform.parent = oldSegment.transform.parent;
		newSegment.transform.localPosition = oldSegment.transform.localPosition;
		segments[index] = newSegment;
		Debug.Log("Destroying old segment " + oldSegment + " from index " + index + ", replaced by " + newSegment);
		Destroy(oldSegment.gameObject);
		return newSegment;
	}
	
	public void DestroyOpponentsSegment(TowerSegment segment) {
		int index = segments.IndexOf(segment);
		m_owningPlayer.DestroyOpponentsSegment(index);
	}
	
	public void DestroyPlayersSegment(int segmentIndex) {
		TowerSegment segment = segments[segmentIndex];
		if (segment.OnIsComplete()) {
			Destroy(segment.gameObject);
			segments.RemoveAt(segmentIndex);
			for (int i = segmentIndex; i < (segments.Count); i++) {
				segments[i].transform.position += new Vector3(0.0f, -2.0f, 0.0f);
				MoveDown();
			}
		}
		else {
			SwapSegment(segmentIndex, m_emptyTowerSegmentPrefab, false);
		}
	}

	public void PerformAction(Tribe tribe)
	{
		TowerSegment segment = segments[m_cursorPosition].GetComponent<TowerSegment>();

		if (!segment.IsActionable() || tribe.IsBusy || tribe.count == 0) {
			/* TODO: Add some kind of notification that the segment cannot be actioned on */
			return;
		}
		
		segment.PerformAction(this, tribe);
	}

	public void OnBeginAction(TowerSegment segment) {
	}

	public void OnProgressAction(TowerSegment segment, float progress, float secondsRemaining) {
	}

	public void OnCompleteAction(TowerSegment segment) {
	}

	public void MoveUp()
	{
		if (m_cursorPosition < (segments.Count - 1))
		{
			m_cursorPosition++;
		}
		m_selector.transform.localPosition = Vector3.up * (float)m_cursorPosition * TowerSegment.HEIGHT;
	}
	
	public void MoveDown()
	{
		if (m_cursorPosition > 0)
		{
			m_cursorPosition--;
		}
		m_selector.transform.localPosition = Vector3.up * (float)m_cursorPosition * TowerSegment.HEIGHT;
	}

	public void MoveLeft()
	{
		// Ensure the empty segment is selected
		EmptyTowerSegment segment = segments[m_cursorPosition].GetComponent<TowerSegment>() as EmptyTowerSegment;
		if (segment == null) return;

		if (m_selectedPrefabIndex > 0) {
			--m_selectedPrefabIndex;
		} else {
			m_selectedPrefabIndex = m_constructableTowerSegments.Count - 1;
		}
		m_prefabSelector.SetSelectedIndex(m_selectedPrefabIndex);
	}
	
	public void MoveRight()
	{
		// Ensure the empty segment is selected
		EmptyTowerSegment segment = segments[m_cursorPosition].GetComponent<TowerSegment>() as EmptyTowerSegment;
		if (segment == null) return;

		if (m_selectedPrefabIndex < (m_constructableTowerSegments.Count - 1)) {
			++m_selectedPrefabIndex;
		} else {
			m_selectedPrefabIndex = 0;
		}
		m_prefabSelector.SetSelectedIndex(m_selectedPrefabIndex);
	}

	private void ShowOrHidePrefabSelector() {
		// Ensure the empty segment is selected
		EmptyTowerSegment segment = segments[m_cursorPosition].GetComponent<TowerSegment>() as EmptyTowerSegment;
		if (segment == null) {
			m_prefabSelector.gameObject.SetActive(false);
		} else {
			m_prefabSelector.gameObject.SetActive(true);
			m_prefabSelector.transform.position = segment.transform.position;
			m_prefabSelector.SetSelectedIndex(m_selectedPrefabIndex);
		}
	}

	public int GetCompletedSegmentCount()
	{
		int completedSegmentCount = 0;
		for(int i=0; i<segments.Count; i++)
		{
			if(segments[i].IsComplete()) {
				completedSegmentCount++;
			}
		}
		return completedSegmentCount;
	}
	
	public void SetOwningPlayer(Player owningPlayer) {
		m_owningPlayer = owningPlayer;
	}
}
