import { useState, type ChangeEvent } from "react";
import { Button } from "../../ui/button";
import api from "../../../utils/api";
import { handleApiError } from "../../../utils/apiErrorHandler";
import { Progress } from "../../ui/progress";

type UploadStatus = "idle" | "uploading" | "success" | "error";

interface FileUploaderProps {
  onUpload: (path: string) => void;
}

export default function FileUploader({ onUpload }: FileUploaderProps) {
  const acceptedFileExtension = ["jpg", "png", "jpeg"];
  const [file, setFile] = useState<File | null>(null);
  const [status, setStatus] = useState<UploadStatus>("idle");
  const [uploadProgress, setUploadProgress] = useState(0);

  const acceptedFileExtensions = ["jpg", "png", "jpeg"];

  const acceptedFileTypesString = acceptedFileExtension
    .map((ext) => `.${ext}`)
    .join(",");

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      validateExtension(e.target.files[0]);
    }
  };

  const validateExtension = (file: File) => {
    const fileTypeRegex = new RegExp(acceptedFileExtensions.join("|"), "i");
    if (!fileTypeRegex.test(file.name.split(".").pop() ?? "")) {
      return;
    }
    setFile(file);
  };

  const handleFileUpload = async () => {
    if (!file) return;

    setStatus("uploading");
    setUploadProgress(0);
    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await api.post("/api/upload", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
        onUploadProgress: (progressEvent) => {
          const progress = progressEvent.total
            ? Math.round((progressEvent.loaded * 100) / progressEvent.total)
            : 0;
          setUploadProgress(progress);
        },
      });
      setStatus("success");
      onUpload(response.data.filename);
      setUploadProgress(100);
    } catch (error) {
      handleApiError(error);
      setStatus("error");
      setUploadProgress(0);
    }
  };

  return (
    <div className="my-2 space-y-2">
      <h3 className="">Téléverser une image</h3>
      <div className="flex">
        <input
          type="file"
          onChange={handleFileChange}
          accept={acceptedFileTypesString}
          className="w-full text-sm file:border-0 file:bg-zinc-800 file:text-white file:rounded-md file:px-4 file:py-2 file:cursor-pointer hover:file:bg-zinc-700"
        />
        {file && status !== "uploading" && (
          <Button
            type="button"
            className="bg-green-500 hover:bg-green-600 cursor-pointer"
            onClick={handleFileUpload}
          >
            Télécharger
          </Button>
        )}
      </div>
      {status == "uploading" && (
        <Progress value={uploadProgress} className="w-[60%]" />
      )}
      {status == "success" && (
        <p className="text-sm text-green-500">
          L'image a été téléchargé avec succès !
        </p>
      )}
      {status == "error" && (
        <p className="text-sm text-red-500">Échec du téléchargement</p>
      )}
    </div>
  );
}
