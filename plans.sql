-- Table: public.plans

-- DROP TABLE IF EXISTS public.plans;

CREATE TABLE IF NOT EXISTS public.plans
(
    name text COLLATE pg_catalog."default" NOT NULL,
    date text COLLATE pg_catalog."default" NOT NULL,
    "time" text COLLATE pg_catalog."default" NOT NULL,
    type text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT plans_pkey PRIMARY KEY (name)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.plans
    OWNER to postgres;