﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;
using Delta.Extensions;
using System.Globalization;
using Delta.Movement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{
    public abstract class Emitter : Entity
    {
        [ContentSerializer]
        string _fadeInMethodString;
        [ContentSerializer]
        string _fadeOutMethodString;
        Interpolation.InterpolationMethod _fadeInInterpolator;
        Interpolation.InterpolationMethod _fadeOutInterpolator;

        [ContentSerializer]
        string _blendString;
        BlendState _blend;

        public float Frequency;
        public bool Explode;
        public int Quantity;
        public Range LifespanRange;
        public Range SpeedRange;
        public Range AccelerationRange;
        public Range RotationRange;
        public Range AngleRange;
        public Range ScaleRange;
        public Range FrameIntervalRange;
        public Range FadeInRange;
        public Range FadeOutRange;

        // fuck you for not serializing blendstates; warning will hardcrash visual studio
        [ContentSerializerIgnore]
        public BlendState Blend
        {
            get
            {
                if (_blend == null)
                    _blend = BlendStateExtensions.Parse(_blendString);
                return _blend;
            }
        }

        // fuck you for not serializing delegates
        [ContentSerializerIgnore]
        public Interpolation.InterpolationMethod FadeInInterpolator
        {
            get
            {
                if (_fadeInInterpolator == null) 
                    _fadeInInterpolator = Interpolation.Parse(_fadeInMethodString); 
                return _fadeInInterpolator;
            }
        }

        // fuck you for not serializing delegates
        [ContentSerializerIgnore]
        public Interpolation.InterpolationMethod FadeOutInterpolator
        {
            get
            {
                if (_fadeOutInterpolator == null) 
                    _fadeOutInterpolator = Interpolation.Parse(_fadeOutMethodString); 
                return _fadeOutInterpolator;
            }
        }

        public Emitter()
        {
            AngleRange = new Range(0, 360);
            ScaleRange = new Range(1);
            Quantity = 1;
            _fadeInMethodString = "Linear";
            _fadeOutMethodString = "Linear";
            _blendString = "AlphaBlend";
        }

        protected internal override bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "frequency":
                    Frequency = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "speed":
                    SpeedRange = Range.Parse(value);
                    return true;
                case "lifespan":
                    LifespanRange = Range.Parse(value);
                    return true;
                case "rotation":
                case "rotationspeed":
                    RotationRange = Range.Parse(value);
                    RotationRange.Lower = RotationRange.Lower.ToRadians();
                    RotationRange.Upper = RotationRange.Upper.ToRadians();
                    return true;
                case "angle":
                    AngleRange = Range.Parse(value);
                    AngleRange.Lower = AngleRange.Lower.ToRadians();
                    AngleRange.Upper = AngleRange.Upper.ToRadians();
                    return true;
                case "scale":
                case "size":
                    ScaleRange = Range.Parse(value);
                    return true;
                case "acceleration":
                    AccelerationRange = Range.Parse(value);
                    return true;
                case "frameinterval":
                case "frameduration":
                    FrameIntervalRange = Range.Parse(value);
                    return true;
                case "explode":
                    Explode = true;
                    Quantity = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "quantity":
                    Quantity = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "fadein":
                    FadeInRange = Range.Parse(value);
                    return true;
                case "fadeout":
                    FadeOutRange = Range.Parse(value);
                    return true;
                case "fadeinmethod":
                    _fadeInMethodString = value;
                    return true;
                case "fadeoutmethod":
                    _fadeOutMethodString = value;
                    return true;
                case "blend":
                    _blendString = value;
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }

        public override void Recycle()
        {
            base.Recycle();
            Frequency = 0;
            Explode = false;
            Quantity = 1;
            LifespanRange = Range.Empty;
            SpeedRange = Range.Empty;
            RotationRange = Range.Empty;
            AngleRange = Range.Empty;
            ScaleRange = new Range(1, 1);
            FadeInRange = Range.Empty;
            FadeOutRange = Range.Empty;
            _fadeInMethodString = "Linear";
            _fadeOutMethodString = "Linear";
            _blendString = "AlphaBlend";
        }

        internal class Particle<T> : IRecyclable where T: Entity
        {
            public Emitter Emitter;

            public T Entity;

            /// <summary>
            /// The amount of time spent living.
            /// </summary>
            public float Life;

            /// <summary>
            /// The total amount of time to live.
            /// </summary>
            public float Lifespan;

            public Vector2 Acceleration;
            public Vector2 Velocity;
            public float AngularVelocity;

            public float FadeOutPercent;
            public float FadeInPercent;

            public bool IsDead { get { return Life >= Lifespan; } }

            public virtual void Recycle()
            {
                Entity.Recycle();
                Entity = null;
                Life = 0;
                Lifespan = 0;
                Acceleration = Vector2.Zero;
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
                FadeOutPercent = 0;
                FadeInPercent = 0;
            }

            public virtual void Update(DeltaTime time)
            {
                if (FadeInPercent > 0)
                    Entity.Alpha = Emitter.FadeInInterpolator(0, 1, Life / (FadeInPercent * Lifespan)); 
                if (FadeOutPercent > 0)
                    Entity.Alpha = Entity.Alpha - Emitter.FadeOutInterpolator(0, 1, (Life - (Lifespan - FadeOutPercent * Lifespan)) / (FadeOutPercent * Lifespan));
            }

            public virtual void Draw(DeltaTime time, SpriteBatch spriteBatch) { }
            public virtual void OnEmitted() { }

        }
    }
}
