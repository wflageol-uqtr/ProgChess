import { useEffect, useState } from "react";
import FileUploader from "../../components/form/input/FileUploader";
import AdminLayout from "../../components/layout/AdminLayout";
import { DataTable } from "../../components/table/Data-table";
import { handleApiError } from "../../utils/apiErrorHandler";
import api from "../../utils/api";
import { ImageColumns } from "../../components/table/ImageColumns";
import { useNavigate } from "react-router";

export default function ListImage() {
  const [image, setImage] = useState([]);
  const navigate = useNavigate();

  const getImages = async () => {
    try {
      const response = await api.get("/api/upload");
      setImage(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getImages();
  }, []);

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col px-4 space-y-4 w-full mt-4">
        <div className="flex w-full">
          <h2 className="text-2xl font-semibold">Liste des images</h2>
        </div>
        <div className="flex">
          <FileUploader onUpload={() => navigate(0)} />
        </div>
        <DataTable
          columns={ImageColumns}
          data={image}
          apiRoute="/api/exercise"
          filter="id"
          showFilter={false}
        />
      </div>
    </AdminLayout>
  );
}
