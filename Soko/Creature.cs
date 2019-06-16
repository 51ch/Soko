using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soko
{
    class Creature
    {
        private int xPosition;
        private int yPosition;
        private Direction _lastDirection;
        private State _currentState;

        public enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }
        public enum State
        {
            Idle,
            Moving
        }

        public int xPos
        {
            get
            {
                return xPosition;
            }
            set
            {
                xPosition = value;
            }
        }
        public int yPos
        {
            get
            {
                return yPosition;
            }
            set
            {
                yPosition = value;
            }
        }
        public Direction lastDirection
        {
            get
            {
                return _lastDirection;
            }
        }
        public State currentState
        {
            get
            {
                return _currentState;
            }
        }
        public Creature(int x, int y)
        {
            xPos = x;
            yPos = y;
            _lastDirection = Direction.Down;
            _currentState = State.Idle;
        }
        public void MovingTo(Direction dir)
        {
            _lastDirection = dir;
            _currentState = State.Moving;
        }

        public void Arrived()
        {
            _currentState = State.Idle;
        }
    }
}
