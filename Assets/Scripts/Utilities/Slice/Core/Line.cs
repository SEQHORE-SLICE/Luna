using UnityEngine;
namespace Utilities.Slice.Core {
	public readonly struct Line {
		private readonly Vector3 _mPosA;
		private readonly Vector3 _mPosB;

		public Line(Vector3 pta, Vector3 ptb) {
			this._mPosA = pta;
			this._mPosB = ptb;
		}

		public float dist {
			get { return Vector3.Distance (this._mPosA, this._mPosB); }
		}

		public float distSq {
			get { return (this._mPosA - this._mPosB).sqrMagnitude; }
		}

		public Vector3 positionA {
			get { return this._mPosA; }
		}

		public Vector3 positionB {
			get { return this._mPosB; }
		}
	}
}