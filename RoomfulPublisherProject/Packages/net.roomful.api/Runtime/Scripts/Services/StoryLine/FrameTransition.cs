


// Copyright Roomful 2013-2018. All rights reserved.



namespace net.roomful.api {

	public abstract class FrameTransition  {

		public abstract string Name { get; }

		public virtual float Duration {
			get {
				return 2f;
			}
		}

    }

}
