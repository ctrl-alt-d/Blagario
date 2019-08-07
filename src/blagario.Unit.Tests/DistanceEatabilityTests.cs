using System;
using Xunit;
using blagario.elements;

namespace blagario.Unit.Tests
{
    public class DistanceEatabilityTests
    {

        private double MassFromRadius(int radius) => Math.Pow( radius,2 )*Math.PI;

        [Fact]
        public void OneContainsOther()
        {
            // taken 2 closed elements 
            var one = new AgarElement() {X=500, Y=500, _Mass=MassFromRadius(11)  } ;
            var other = new AgarElement() {X=500, Y=500, _Mass=MassFromRadius(9) } ;

            // When check for eatability
            var canEat = ElementsHelper.CanOneElementEatsOtherOneByDistance(one,other);

            // One must be able to eat the other one
            Assert.True(canEat);
        }

        [Fact]
        public void OneCloserOther()
        {
            /*                                             

                |<---------15-------->|                                                                                                
                |<----10---->|                                                                                                
                o ---- (---- o ----)--)
                       |<-2->|

            */

            // taken 2 closed elements 
            var one = new AgarElement() {X=500, Y=100, _Mass=MassFromRadius(15)  } ;
            var other = new AgarElement() {X=510, Y=100, _Mass=MassFromRadius(2) } ;

            // When check for eatability
            var canEat = ElementsHelper.CanOneElementEatsOtherOneByDistance(one,other);

            // One must be able to eat the other one
            Assert.True(canEat);
        }


        [Fact]
        public void OneOutOfTheOther()
        {
            /*                                             

                |<-----12------->|                                                                                                
                |<----10---->|                                                                                                
                o ---- (---- o --)-)
                       |<-4->|<-4->|

            */

            // taken 2 closed elements 
            var one = new AgarElement() {X=500, Y=100, _Mass=MassFromRadius(12)  } ;
            var other = new AgarElement() {X=510, Y=100, _Mass=MassFromRadius(4) } ;

            // When check for eatability
            var canEat = ElementsHelper.CanOneElementEatsOtherOneByDistance(one,other);

            // One must be able to eat the other one
            Assert.False(canEat);
        }

        [Fact]
        public void OneCloserOtherDiagonal()
        {
            /*                                             
                |-2-|
               -o  
               | \
               2  \/
               |  /\
               -    o
                     \/
                     /
                     

            */

            // taken 2 closed elements 
            var one = new AgarElement() {X=500, Y=100, _Mass=MassFromRadius( (int)Math.Sqrt( 3*3 + 3*3 ) )  } ;
            var other = new AgarElement() {X=502, Y=102, _Mass=MassFromRadius(1) } ;

            // When check for eatability
            var canEat = ElementsHelper.CanOneElementEatsOtherOneByDistance(one,other);

            // One must be able to eat the other one
            Assert.True(canEat);
        }

        [Fact]
        public void OneFarAwayOtherDiagonal()
        {
            /*                                             
                |-2-|
               -o  
               | \
               2  \/
               |  /\
               -    o
                     \/
                     /
                     

            */

            // taken 2 closed elements 
            var one = new AgarElement() {X=500, Y=100, _Mass=MassFromRadius( (int)Math.Sqrt( 2*2 + 2*2 ) )  } ;
            var other = new AgarElement() {X=502, Y=102, _Mass=MassFromRadius(1) } ;

            // When check for eatability
            var canEat = ElementsHelper.CanOneElementEatsOtherOneByDistance(one,other);

            // One must be able to eat the other one
            Assert.False(canEat);
        }

    }
}
