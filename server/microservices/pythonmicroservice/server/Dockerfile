# FROM python:3.10-alpine

# WORKDIR /app

# COPY . ./

# ENTRYPOINT ["python", "server.py"]

FROM python:3.10-slim as compiler
ENV PYTHONUNBUFFERED 1

WORKDIR /app/

RUN python -m venv /opt/venv
# Enable venv
ENV PATH="/opt/venv/bin:$PATH"

COPY ./requirements.txt /app/requirements.txt
RUN pip install -Ur requirements.txt

FROM python:3.10-slim as runner
WORKDIR /app/
COPY --from=compiler /opt/venv /opt/venv

# Enable venv
ENV PATH="/opt/venv/bin:$PATH"
COPY . /app/

CMD /bin/bash ./entrypoint.sh
# CMD ["python", "server.py"]