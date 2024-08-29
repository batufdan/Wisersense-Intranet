-- Table: public.users

-- DROP TABLE IF EXISTS public.users;

CREATE TABLE IF NOT EXISTS public.users
(
    email text COLLATE pg_catalog."default" NOT NULL,
    password text COLLATE pg_catalog."default" NOT NULL,
    fullname text COLLATE pg_catalog."default" NOT NULL,
    birthdate text COLLATE pg_catalog."default" NOT NULL,
    admin integer NOT NULL,
    image text COLLATE pg_catalog."default",
    CONSTRAINT users_pkey PRIMARY KEY (email)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.users
    OWNER to postgres;