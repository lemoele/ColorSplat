using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using TUIO;

public class TuioTest1 : MonoBehaviour, TuioListener {
	
	private TuioClient client;
	private int port = 3333;
	
	private float Xpos;
	private float Ypos;

	private int prevId = 0;
	private int currId = 0;

	// Use this for initialization
	void Start () {
		client = new TuioClient(port);
		client.addTuioListener(this);
		client.connect();
		Debug.Log (client.isConnected());
	}
	
	// Update is called once per frame
	void Update () {
		if (client.getTuioCursors().Count > 0) {
			Debug.Log ("Count : " + client.getTuioCursors().Count);
			currId = (int) client.getTuioCursors()[0].SessionID;
			if (prevId != currId) {
				Xpos = client.getTuioCursors()[0].X;
				Ypos = client.getTuioCursors()[0].Y;
				prevId = currId;
			} else {
				Xpos = Ypos = -1.0f;
			}
			//Debug.Log ("Id : " + (client.getTuioCursors()[0]).CursorId);
			Debug.Log ("Cursor : " + Xpos + " " +  Ypos);
		} else {
			Xpos = Ypos = -1.0f;
		}
		this.GetComponent<ThrowBall>().tuioX = Xpos;
		this.GetComponent<ThrowBall>().tuioY = Ypos;
	}

	public void addTuioObject(TuioObject o) {
		client.getTuioObjects().Add(o);
	}
	
	public void updateTuioObject(TuioObject o) {
		client.getTuioObject(o.SessionID).update(o);
	}
	
	public void removeTuioObject(TuioObject o) {
		client.getTuioObjects().Remove(client.getTuioObject(o.SessionID));
	}
	
	public void addTuioCursor(TuioCursor c) {
		client.getTuioCursors().Add(c);
	}
	
	public void updateTuioCursor(TuioCursor c) {
		client.getTuioCursor(c.SessionID).update(c);
	}

	public void removeTuioCursor(TuioCursor c) {
		client.getTuioCursors().Remove(client.getTuioCursor(c.SessionID));
	}

	public void addTuioBlob(TuioBlob b) {
		client.getTuioBlobs().Add(b);
	}
	
	public void updateTuioBlob(TuioBlob b) {
		client.getTuioBlob(b.SessionID).update(b);
	}
	
	public void removeTuioBlob(TuioBlob b) {
		client.getTuioBlobs().Remove(client.getTuioBlob(b.SessionID));
	}

	public void refresh(TuioTime frameTime) {
	}

}
