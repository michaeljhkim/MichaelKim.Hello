FROM quay.io/pypa/manylinux_2_34_x86_64

WORKDIR /workspaces


# Install necessary build tools and libraries
RUN dnf install -y \
    krb5-libs libicu openssl-libs zlib \
    aspnetcore-runtime-9.0 aspnetcore-runtime-dbg-9.0 aspnetcore-targeting-pack-9.0 dotnet-apphost-pack-9.0 dotnet-host dotnet-hostfxr-9.0 \
    dotnet-runtime-9.0 dotnet-runtime-dbg-9.0 dotnet-sdk-9.0 dotnet-sdk-9.0-source-built-artifacts dotnet-sdk-aot-9.0 dotnet-sdk-dbg-9.0 \
    dotnet-targeting-pack-9.0 dotnet-templates-9.0 netstandard-targeting-pack-2.1

RUN rpm -i https://ftp.postgresql.org/pub/pgadmin/pgadmin4/yum/pgadmin4-redhat-repo-2-1.noarch.rpm

RUN dnf install -y \
    postgresql pgadmin4
    
RUN dnf -y module install nodejs:20

RUN dnf config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo && \
    dnf install -y docker

RUN dotnet new install Aspire.ProjectTemplates


# Default command: Start bash to interact with the container
CMD ["/bin/bash"]