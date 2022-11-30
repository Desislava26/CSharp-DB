USE [Geography]

SELECT Peaks.PeakName, Peaks.Elevation, Mountains.MountainRange
FROM     Mountains INNER JOIN
                  Peaks ON Mountains.Id = Peaks.MountainId
WHERE  (Mountains.MountainRange = N'Rila')
ORDER BY Peaks.Elevation DESC