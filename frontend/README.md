## How long did you spend on your solution?
    1,5 Hours (Only front end funtion, Missing backend connections that can be done with fetch)

## How do you build and run your solution?
    - npm install
    - npm start (For run without build)
    - npm run build (For build the app and run on production)

## What technical and functional assumptions did you make when implementing your solution?
    In the solution I try to reutilize the most components as posible, to do it cleaner and easy to maintain.
    And I have use boostrap to make a friendly app, without takes much time.
    The filosofy was do the most functionality and friendly app with the less code as posible (reusing all as possible)

## Explain briefly your technical design and why do you think is the best approach to this problem.
    The desing It's very simple, many components that control a part of the aplication.
    The main component (App), have the general functions that are going to be used for the other components, I think It's better to manage the main functions on the parent Components, then the child component, use them, this is how we can get a code easier to mantain, cleaner and reusable

## IMPORTANT
    On src/App.jsx have a constant (app_url) that contain the app url, is necesary to run the connections.