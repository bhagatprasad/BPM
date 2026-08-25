CREATE TABLE public.roles
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    name character varying(100) COLLATE pg_catalog."default" NOT NULL,
    code character varying(50) COLLATE pg_catalog."default" NOT NULL,
    description character varying(255) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT roles_pkey PRIMARY KEY (id),
    CONSTRAINT roles_code_key UNIQUE (code)
)
