-- Postgres init script

-- Create the hello_info table
CREATE TABLE IF NOT EXISTS hello_info (
    person_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    age INT NOT NULL,
    email VARCHAR(50) NOT NULL,
    github VARCHAR(50) NOT NULL,
    birth_date DATE NOT NULL
);

-- Insert some sample data into the hello_info table
INSERT INTO hello_info (first_name, last_name, age, email, github, birth_date)
VALUES ('Michael', 'Kim', 21, 'michaelkimwork47@gmail.com', 'https://github.com/michaeljhkim', '2003-09-08')
ON CONFLICT DO NOTHING;