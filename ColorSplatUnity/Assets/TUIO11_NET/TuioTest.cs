using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using TUIO;

public class TuioTest : MonoBehaviour, TuioListener {
	
	private TuioClient client;
	private Dictionary<long,TuioObject> objectList;
	private Dictionary<long,TuioCursor> cursorList;
	private Dictionary<long,TuioBlob> blobList;
	private object cursorSync = new object();
	private object objectSync = new object();
	private object blobSync = new object();
	private int port = 3333;

	private bool verbose;
	
	private float Xpos;
	private float Ypos;

	// Use this for initialization
	void Start () {
		verbose = true;

		objectList = new Dictionary<long,TuioObject>(128);
		cursorList = new Dictionary<long,TuioCursor>(128);
		blobList = new Dictionary<long,TuioBlob>(128);
		
		client = new TuioClient(port);
		client.addTuioListener(this);
		client.connect();
	}
	
	// Update is called once per frame
	void Update () {
		if (cursorList.Count > 0) {
			lock(cursorSync) {
				foreach (TuioCursor tcursor in cursorList.Values) {
					Xpos = tcursor.X;
					Ypos = tcursor.Y;
				}
			}
		} else {
			Xpos = Ypos = -1.0f;
		}
		this.GetComponent<ThrowBall>().tuioX = Xpos;
		this.GetComponent<ThrowBall>().tuioY = Ypos;
		cursorList.Clear();
	}

	public void addTuioObject(TuioObject o) {
		lock(objectSync) {
			objectList.Add(o.SessionID,new TuioObject(o));
		} if (verbose) Console.WriteLine("add obj "+o.SymbolID+" ("+o.SessionID+") "+o.X+" "+o.Y+" "+o.Angle);
	}
	
	public void updateTuioObject(TuioObject o) {
		lock(objectSync) {
			objectList[o.SessionID].update(o);
		}
		if (verbose) Console.WriteLine("set obj "+o.SymbolID+" "+o.SessionID+" "+o.X+" "+o.Y+" "+o.Angle+" "+o.MotionSpeed+" "+o.RotationSpeed+" "+o.MotionAccel+" "+o.RotationAccel);
	}
	
	public void removeTuioObject(TuioObject o) {
		lock(objectSync) {
			objectList.Remove(o.SessionID);
		}
		if (verbose) Console.WriteLine("del obj "+o.SymbolID+" ("+o.SessionID+")");
	}
	
	public void addTuioCursor(TuioCursor c) {
		lock(cursorSync) {
			cursorList.Add(c.SessionID,c);
		}
		if (verbose) Console.WriteLine("add cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y);
	}
	
	public void updateTuioCursor(TuioCursor c) {
		if (verbose) Console.WriteLine("set cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y+" "+c.MotionSpeed+" "+c.MotionAccel);
	}
	
	public void removeTuioCursor(TuioCursor c) {
		lock(cursorSync) {
			cursorList.Remove(c.SessionID);
		}
		if (verbose) Console.WriteLine("del cur "+c.CursorID + " ("+c.SessionID+")");
	}

	public void addTuioBlob(TuioBlob b) {
		lock(blobSync) {
			blobList.Add(b.SessionID,b);
		}
		if (verbose) Console.WriteLine("add blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area);
	}
	
	public void updateTuioBlob(TuioBlob b) {
		if (verbose) Console.WriteLine("set blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area+" "+b.MotionSpeed+" "+b.RotationSpeed+" "+b.MotionAccel+" "+b.RotationAccel);
	}
	
	public void removeTuioBlob(TuioBlob b) {
		lock(blobSync) {
			blobList.Remove(b.SessionID);
		}
		if (verbose) Console.WriteLine("del blb "+b.BlobID + " ("+b.SessionID+")");
	}

	public void refresh(TuioTime frameTime) {
//		Invalidate();
	}

}
