select e.Title,Description, Location,CreatedAt,u.FirstName + u.LastName as OwnerName
from Events as e
join Users as u on e.OwnerId= u.Id
JOIN Hobbies AS H ON E.HobbyId = H.Id

SELECT Title, c.ID, Content, Username
FROM Comments AS c
JOIN Events AS e ON  c.EventID = e.Id
JOIN Users AS u ON  c.UserID = u.Id

SELECT Title,Username, Role
FROM EventParticipants AS EP
JOIN EventS AS E ON EP.EventId = E.Id
JOIN Users AS U ON EP.UserId = U.Id


SELECT E.Id, COUNT(T.Id) AS NumberOfTasks
FROM EventTasks AS ET 
JOIN Events AS E ON E.Id=ET.EVENTID
JOIN Tasks AS T ON T.ID = ET.TasksId
GROUP BY E.Id

SELECT u.Username, r.Name
from Users as u
JOIN UserRole as ur on ur.UserId = u.Id
JOIN Roles as r on r.Id = ur.RoleId


SELECT * 
FROM Events

SELECT *
FROM Hobbies

SELECT *
FROM Users

SELECT *
FROM EventParticipants

SELECT *
FROM Comments

SELECT * 
FROM Tasks

SELECT * 
FROM EventTasks


SELECT * 
FROM Users;

SELECT * 
FROM Roles;




