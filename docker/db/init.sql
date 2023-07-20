\connect TodoDb

DROP TABLE IF EXISTS Book;
DROP TABLE IF EXISTS Author;
DROP TABLE IF EXISTS BookAuthor;
DROP TABLE IF EXISTS Genre;
DROP TABLE IF EXISTS BookGenre;

-- Table: Book
CREATE TABLE Book (
    id SERIAL PRIMARY KEY,
    isbn character varying(17) NOT NULL UNIQUE,
    title character varying(255) NOT NULL,
    description character varying(3500) NOT NULL,
    publisher character varying(255),
    publication_year integer NOT NULL,
    place_of_publication character varying(255),
    number_of_pages integer NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    small_thumbnail bytea,
    large_thumbnail bytea,
    sum_of_stars integer NOT NULL,
    review_count integer NOT NULL,
    --average_stars numeric(2,1) NOT NULL 
    product_hotness NUMERIC(30, 10),
    hotness_date timestamp with time zone,
    average_stars numeric(2,1) GENERATED ALWAYS AS (ROUND(sum_of_stars::decimal / NULLIF(review_count, 0), 1)) STORED --two digits total; one behind the decimal point
);

-- Table: Author
CREATE TABLE Author (
    id SERIAL PRIMARY KEY,
    name character varying(255) NOT NULL UNIQUE
);

CREATE TABLE BookAuthor (
    book_id integer NOT NULL,
    author_id integer NOT NULL
);

ALTER TABLE BookAuthor OWNER TO postgres;
ALTER TABLE BookAuthor ADD CONSTRAINT "BookAuthor_PKEY" PRIMARY KEY (book_id, author_id);

-- Table: Genre
CREATE TABLE Genre (
    id SERIAL PRIMARY KEY,
    name character varying(50) NOT NULL
);

CREATE TABLE BookGenre (
    book_id integer NOT NULL,
    genre_id integer NOT NULL
);

ALTER TABLE BookGenre OWNER TO postgres;
ALTER TABLE BookGenre ADD CONSTRAINT "BookGenre_PKEY" PRIMARY KEY (book_id, genre_id);

--Table: Review
CREATE TABLE Review (
    id SERIAL PRIMARY KEY,
    book_id integer NOT NULL,
    user_Id integer NOT NULL,
    stars integer NOT NULL,
    content character varying(10000),
    thumbs_up integer DEFAULT 0 NOT NULL,
    thumbs_down integer DEFAULT 0 NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE INDEX idx_review_book_id ON Review(book_id);

--Table: Comment
CREATE TABLE Comment (
    id SERIAL PRIMARY KEY,
    review_id integer NOT NULL,
    reply_comment_id integer,
    user_Id integer NOT NULL,
    content character varying(10000),
    thumbs_up integer DEFAULT 0 NOT NULL,
    thumbs_down integer DEFAULT 0 NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE INDEX idx_comment_review_id ON Comment(review_id);

--Table: UserAccount
CREATE TABLE UserAccount (
    id SERIAL PRIMARY KEY,
    user_name character varying(255) NOT NULL UNIQUE,
    email character varying(255) NOT NULL UNIQUE,
    first_name character varying(50) NOT NULL,
    last_name character varying(50) NOT NULL,
    birth_date DATE NOT NULL,
    password_hash character varying(100) NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);

INSERT INTO Book (isbn, title, description, publisher, publication_year, place_of_publication, number_of_pages, sum_of_stars, review_count)
    VALUES ('9780321877581', 'Pattern Recognition and Machine Learning', '', 'Springer', 2014, '', 0, 7, 3);
INSERT INTO Book (isbn, title, description, publisher, publication_year, place_of_publication, number_of_pages, sum_of_stars, review_count)
    VALUES ('9783423218054', 'Der Oktobermann', 'Gute Beschreibung', 'dtv', 2019, 'München', 160, 14, 3);
INSERT INTO Book (isbn, title, description, publisher, publication_year, place_of_publication, number_of_pages, sum_of_stars, review_count)
    VALUES ('9783423215077', 'Der böse Ort', 'Dieser Ort wirkte zunächst sehr freundlich, doch eines Tages kam die Wahreheit ans Licht!', 'dtv', 2017, 'München', 216, 15, 4);
INSERT INTO Book (isbn, title, description, publisher, publication_year, place_of_publication, number_of_pages, sum_of_stars, review_count)
    VALUES ('9783833936340', 'Gregs Tagebuch 3 - Jetzt reichts!', 'Der dritte Teil des erfolgreichen Typs', 'Kosmos', 2013, 'Düsseldorf', 219, 40, 10);
INSERT INTO Book (isbn, title, description, publisher, publication_year, place_of_publication, number_of_pages, sum_of_stars, review_count)
    VALUES ('9783608932225', 'Der Herr der Ringe', 'Wat is Wacken?', 'Klett-Cotta', 2000, '', 1215, 50, 10);
INSERT INTO Author (name)
    VALUES ('Christopher M. Bishop');
INSERT INTO Genre (name)
    VALUES ('Machine Learning');
INSERT INTO Genre (name)
    VALUES ('Roman');
INSERT INTO Genre (name)
    VALUES ('Weltbild');
INSERT INTO Genre (name)
    VALUES ('Krimi');
INSERT INTO BookAuthor (book_id, author_id)
    VALUES (1, 1);
INSERT INTO BookGenre (book_id, genre_id)
    VALUES (1, 1);

INSERT INTO UserAccount (user_name, email, first_name, last_name, birth_date, password_hash)
    VALUES ('Mr_X', 'mrx@gmail.com', 'Max', 'Mustermann', '01.01.2001', '');
INSERT INTO UserAccount (user_name, email, first_name, last_name, birth_date, password_hash)
    VALUES ('Mr_A', 'mra@gmail.com', 'Maxa', 'Mustermanna', '01.01.2001', '');
INSERT INTO UserAccount (user_name, email, first_name, last_name, birth_date, password_hash)
    VALUES ('Mr_B', 'mrb@gmail.com', 'Maxb', 'Mustermannb', '01.01.2001', '');

INSERT INTO Review (book_id, user_Id, stars, content, thumbs_up, thumbs_down)
    VALUES (1, 1, 3, 'This book was a-okay!', 2, 0);

INSERT INTO Comment (review_id, reply_comment_id, user_Id, content, thumbs_up, thumbs_down)
    VALUES (1, NULL, 2, 'Thought the same!', 0, 2);
INSERT INTO Comment (review_id, reply_comment_id, user_Id, content, thumbs_up, thumbs_down)
    VALUES (1, 1, 1, 'Useless comment!', 5, 0);
INSERT INTO Comment (review_id, reply_comment_id, user_Id, content, thumbs_up, thumbs_down)
    VALUES (1, 2, 2, 'Now come on?', 0, 2);
INSERT INTO Comment (review_id, reply_comment_id, user_Id, content, thumbs_up, thumbs_down)
    VALUES (1, NULL, 3, 'Yeah right?!', 0, 0);
