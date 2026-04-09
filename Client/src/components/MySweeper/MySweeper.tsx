import React from 'react';

import { Swiper, SwiperSlide } from 'swiper/react';
import { Navigation, Pagination, Autoplay } from 'swiper/modules';
import { useNavigate } from 'react-router-dom';
import { Game } from "../../Model/Game"

import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';


interface MySweeperProps {
    games?: Game[];
    screenshots?: string[];
    isSingle?: boolean;
}


const MySweeper: React.FC<MySweeperProps> = ({ games, screenshots, isSingle }) => {


    const nev = useNavigate();


    return (
        <Swiper
            modules={[Navigation, Autoplay]}
            slidesPerView={isSingle ? 1 : 3}
            spaceBetween={0}
            navigation
            pagination={{ clickable: true }} // נבטל את הנקודות בעמוד המשחק
            autoplay={{ delay: 3000 }}
            breakpoints={isSingle ? {} : {
                0: { slidesPerView: 1 },
                640: { slidesPerView: 2 },
                1024: { slidesPerView: 3 },
            }}
        // className={isSingle ? "single-swiper" : "multi-swiper"}
        >
            {games?.map((game) => (
                <SwiperSlide key={game.id} onClick={() => nev(`/game/${game.id}`)}>
                    <img src={game.imageURL} alt={game.title} className="swiper-img" />
                </SwiperSlide>
            ))}


            {screenshots?.map((image,index) => (
                <SwiperSlide key={`${image}-${index}`}>
                    <img src={image} alt="screenshot" className="swiper-img" />
                </SwiperSlide>
            ))}






        </Swiper>
    )

}
export default MySweeper;