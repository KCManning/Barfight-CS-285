using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarFightEngineM2
{
    public class Origen
    {
        private float origenX, origenY, origenZ;
        private Collision_Type type;

        /// <summary>
        /// defualt constructor--sets all fields to 0;
        /// </summary>
        public Origen()
        {
            origenX = origenY = origenZ = 0;
        }


        /// <summary>
        /// overloaded constructor--sets all fields tot he passed in parameters
        /// </summary>
        /// <param name="origenX"></param>
        /// <param name="origenY"></param>
        /// <param name="origenZ"></param>
        public Origen(float origenX, float origenY, float origenZ)
        {
            this.origenX = origenX;
            this.origenY = origenY;
            this.origenZ = origenZ;
        }

        /// <summary>
        /// The X Coord of the origen
        /// </summary>
        public float OrigenX
        {
            get
            {
                return origenX;
            }
            set
            {
                origenX = value;
            }
        }

        /// <summary>
        /// The Y Coord of the origen
        /// </summary>
        public float OrigenY
        {
            get
            {
                return origenY;
            }
            set
            {
                origenY = value;
            }
        }

        /// <summary>
        /// The Z Coord of the origen
        /// </summary>
        public float OrigenZ
        {
            get
            {
                return origenZ;
            }
            set
            {
                origenZ = value;
            }
        }

        public Collision_Type Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }


        /// <summary>
        /// The collision type for this origen shape
        /// </summary>
        public enum Collision_Type
        {
            NONE,
            AABB,
            BS,
            BC
        }
    }

    /// <summary>
    /// An axis aligned bounding box class
    /// </summary>
    public class AABB : Origen
    {
        private float halfWidth, halfLength, halfHeight;
        /// <summary>
        /// defualt constructor-- sets all fields to 0
        /// </summary>
        public AABB() : base()
        {
            halfWidth = halfLength = halfHeight = 0;
        }

        /// <summary>
        /// overloaded constructor-- set fields to the inputed parameters
        /// </summary>
        /// <param name="origenX"> X coord of the AABB's center</param>
        /// <param name="origenY"> Y coord of the AABB's center</param>
        /// <param name="origenZ"> Z coord of the AABB's center</param>
        /// <param name="halfWidth">The halfwidth along the X axis of this AABB</param>
        /// <param name="halfLength">The halfwidth along the Y axis of this AABB</param>
        /// <param name="halfHeight">The halfwidth along the Z axis of this AABB</param>
        public AABB(float origenX, float origenY, float origenZ, float halfWidth, float halfLength, float halfHeight) : base(origenX, origenY, origenZ)
        {
            this.halfWidth = halfWidth;
            this.halfLength = halfLength;
            this.halfHeight = halfHeight;
        }

        public bool collides(AABB other)
        {

            //check to se if this AABB collides with the other AABB

            return (Math.Abs(OrigenZ - other.OrigenZ) <= halfHeight + other.halfHeight && 
                Math.Abs(OrigenX - other.OrigenX) <= halfWidth + other.HalfWidth &&
                Math.Abs(OrigenY - other.OrigenY) <= HalfLength + other.HalfLength);

        }

        /// <summary>
        /// The halfwidth of this AABB along the X axis
        /// </summary>
        public float HalfWidth
        {
            get
            {
                return halfWidth;
            }
            set
            {
                halfWidth = value;
            }
        }

        /// <summary>
        /// The halfwidth of this AABB along the Y axis
        /// </summary>
        public float HalfLength
        {
            get
            {
                return halfLength;
            }
            set
            {
                halfLength = value;
            }
        }

        /// <summary>
        /// The halfwidth of this AABB along the Z axis
        /// </summary>
        public float HalfHeight
        {
            get
            {
                return halfHeight;
            }
            set
            {
                halfHeight = value;
            }
        }


    }


    /// <summary>
    /// A bounding cylinder class
    /// </summary>
    public class BC : Origen
    {
        private float radius, halfHeight;

        /// <summary>
        /// defualt constructor-- sets all fields to 0
        /// </summary>
        public BC() : base()
        {
            radius = halfHeight = 0;
        }


        /// <summary>
        /// overloaded constructor-- sets all fields to there inputed parameters
        /// </summary>
        /// <param name="radius"> the radius of this BC</param>
        /// <param name="origenX"> the X coord of the center of this BC</param>
        /// <param name="origenY"> the Y coord of the center of this BC</param>
        /// <param name="origenZ"> the Z coord of the center of this BC</param>
        /// <param name="halfHeight"> the halfwidth along the Z axis of this BC</param>
        public BC(float radius, float origenX, float origenY, float origenZ, float halfHeight) : base(origenX, origenY, origenZ)
        {

            this.halfHeight = halfHeight;
        }


        /// <summary>
        /// The radius of this BC
        /// </summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        /// <summary>
        /// The halfwidth along the Z axis of this BC
        /// </summary>
        public float HalfHeight
        {
            get
            {
                return halfHeight;
            }
            set
            {
                halfHeight = value;
            }
        }

    }



    /// <summary>
    /// A bounding sphere class
    /// </summary>
    class BS : Origen
    {
        private float radius;


        /// <summary>
        /// defualt constructor--Set all fields to 0
        /// </summary>
        public BS() : base()
        {
            radius = 0;
        }

        /// <summary>
        /// overloaded constructor-- Sets fields ot he inputed parameters
        /// </summary>
        /// <param name="origenX"></param>
        /// <param name="origenY"></param>
        /// <param name="origenZ"></param>
        /// <param name="radius"></param>
        public BS(float origenX, float origenY, float origenZ, float radius) : base(origenX, origenY, origenZ)
        {

            this.radius = radius;
        }

        /// <summary>
        /// The radius of this BS
        /// </summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }


    }

    public class Octree : Origen
    {
        Octree[][][] nodes;
        Origen[] bucket;
        byte count;

        /// <summary>
        /// defualt constructor--constructs an empty octree
        /// </summary>
        public Octree(float origenX, float origenY, float origenZ) : base(origenX, origenY, origenZ)
        {
            //construct the tree and its nodes
            nodes = new Octree[2][][];
            for (int i = 0; i < 2; i++)
            {
                nodes[i] = new Octree[2][];
                for (int j = 0; j < 2; j++)
                {
                    nodes[i][j] = new Octree[2];
                }
            }
            bucket = null;
        }

        /// <summary>
        /// inserts the shape into the tree
        /// </summary>
        /// <param name="shape"></param>
        public void insert(Origen shape)
        {
            //insert the shape if there is no shape in this branch of the tree
            if (null == null)
            {
                // data = shape;
            }
            else
            {
                //theres data in this branch so decide which branch the shape will need to go to
                byte x, y, z;
                short ix, iy, iz;

                //check to see which side its on on the x axis
                if (shape.OrigenX > OrigenX)
                {
                    x = 1;
                    ix = 1;
                }
                else
                {
                    x = 0;
                    ix = -1;
                }

                if (shape.OrigenY > OrigenY)
                {
                    y = 1;
                    iy = 1;
                }
                else
                {
                    y = 0;
                    iy = -1;
                }

                if (shape.OrigenZ > OrigenZ)
                {
                    z = 1;
                    iz = 1;
                }
                else
                {
                    z = 0;
                    iz = -1;
                }

                //check to see if the branch in x,y,z is null and if it is construct it
                if (nodes[x][y][z] == null)
                {
                    nodes[x][y][z] = new Octree(OrigenX + ix, OrigenY + iy, OrigenZ + iz);
                }

                //insert in the new tree
                nodes[x][y][z].insert(shape);
            }
        }


        /// <summary>
        /// Returns the shapes in the bucket of the most probale 
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public Origen[] getCollisions(Origen shape)
        {
            List<Origen> shapes = new List<Origen>();
            return getCollisions(shape, shapes).ToArray();
        }

        private List<Origen> getCollisions(Origen shape, List<Origen> shapes)
        {
            return null;
        }
    }
}
