import { useEffect, useState } from "react";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "../../ui/select";
import type { Image } from "../../../utils/type";
import { handleApiError } from "../../../utils/apiErrorHandler";
import api from "../../../utils/api";
import { Link } from "react-router";

interface ImageSelectorProps {
  onSelectedImage: (e?: Image) => void;
}

export default function ImageSelector({ onSelectedImage }: ImageSelectorProps) {
  const [images, setImages] = useState<Image[]>([]);

  const getUserImage = async () => {
    try {
      const response = await api.get("/api/upload");
      setImages(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getUserImage();
  }, []);

  const handleSelectChange = (e: any) => {
    let image = images.find((item) => item.id == e);
    onSelectedImage(image);
  };

  return (
    <Select onValueChange={handleSelectChange}>
      <SelectTrigger className="w-[180px]">
        <SelectValue placeholder="Choisir une image" />
      </SelectTrigger>
      <SelectContent className="bg-zinc-900 text-white border border-zinc-700">
        <SelectGroup>
          <SelectLabel className="text-zinc-400 px-3 py-1">Images</SelectLabel>
          {images.map((img: Image, index) => (
            <SelectItem
              key={index}
              className="cursor-pointer hover:bg-zinc-700 px-3 py-2"
              value={img.id.toString()}
            >
              {img.name}
            </SelectItem>
          ))}
          <div className="text-sm px-3 py-2 block">
            Ajouter des images{" "}
            <Link
              className="text-blue-500 hover:text-blue-600 hover:underline"
              to="/admin/image"
            >
              ici
            </Link>
          </div>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
}
